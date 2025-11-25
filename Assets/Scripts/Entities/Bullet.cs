using Enums;
using Interfaces;
using Managers;
using Pool;
using Scriptables;
using Scriptables.Entities;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Bullet : MonoBehaviour, IPoolable, IPausable
    {
        [SerializeField] private BulletData bulletData;
        
        public Entity Owner { get; set; }
     
        private Rigidbody _rigidbody;
        private Collider _collider;
        
        private TeamType _ownerTeamType;
        private Vector3 _direction;
        private Vector3 _currentAngularVelocity;
        
        private bool _isMovementStopped;
        private bool _isFinallyStopped;
            
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            
            EnableBullet(false);
        }

        private void Start()
        {
            EventManager.GameEvents.Pause += PauseEntity;
        }

        private void FixedUpdate()
        {
            if (gameObject.activeInHierarchy && !_isMovementStopped)
            {
                transform.position += _direction * Time.fixedDeltaTime;
            }
        }

        public void Activate()
        {
            EnableBullet(true);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            
            transform.parent = null;
        }

        public void Fire(Vector3 direction, Vector3 velocity)
        {
            _rigidbody.velocity += velocity;
            
            _direction = direction * bulletData.bulletForce;
            _rigidbody.AddTorque(transform.right, ForceMode.Impulse);
        }

        public void Bounce(float force, Entity newOwner)
        {
            SetOwner(newOwner);
            
            const int errorAngle = 2; 

            var randomX = Random.Range(-errorAngle, errorAngle);
            var randomY = Random.Range(-errorAngle, errorAngle);
            
            var spreadRotation = Quaternion.Euler(randomX, randomY, 0);
            var deviatedDirection = spreadRotation * -_direction.normalized * force;
            
            _direction = deviatedDirection;
            _rigidbody.AddTorque(transform.right, ForceMode.Impulse);
        }

        public void SetOwner(Entity owner)
        {
            Owner = owner;
            _ownerTeamType = owner.GetTeam();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var hitCollider = collision.collider;
            if (hitCollider.TryGetComponent(out Bullet _))
            {
                _rigidbody.isKinematic = false;
                return;
            }

            hitCollider.TryGetComponent(out Entity hitEntity);
            if (hitEntity && hitEntity != Owner)
            {
                if (hitEntity.GetAttributesComponent().IsShielded())
                {
                    Bounce(bulletData.bulletForce / 2f, hitEntity);
                    ApplyHit(collision, hitEntity);

                    return;
                }

                if (hitEntity.GetTeam() != _ownerTeamType)
                    transform.SetParent(hitEntity.transform);

                ApplyHit(collision, hitEntity);
            }

            if (hitEntity == Owner) return;
            
            EnableBullet(false);
            _isFinallyStopped = true;
            
            ReturnToPool();
        }

        private void ApplyHit(Collision collision, Entity hitEntity)
        {
            hitEntity.TakeDamage(bulletData.damage);

            hitEntity.GetHit(
                _direction.normalized,
                collision.GetContact(0).normal,
                bulletData.bulletForce
            );
        }

        private void EnableBullet(bool enable)
        {
            if (enable)
            {
                _rigidbody.isKinematic = false;
                _rigidbody.angularVelocity = _currentAngularVelocity;  
            }
            else
            {
                _currentAngularVelocity = _rigidbody.angularVelocity;
                _rigidbody.isKinematic = true;
            }
            
            _isMovementStopped = !enable;
            _collider.enabled = enable;
        }

        private void ReturnToPool()
        {
            var corr = FactoryManager.Instance.ReturnObjectWithLifeTime(
                bulletData.poolableType,
                this,
                bulletData.timeBeforeDeactivate
            );

            StartCoroutine(corr);
        }

        public void PauseEntity(bool pause)
        {
            if (_isFinallyStopped) return;
            
            EnableBullet(!pause);
        }
        
        private void OnDestroy()
        {
            EventManager.GameEvents.Pause -= PauseEntity;
        }
    }
}