using Enums;
using Interfaces;
using Managers;
using Pool;
using Scriptables;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Bullet : MonoBehaviour, IPoolable, IPausable
    {
        [SerializeField] private BulletData bulletData;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private Entity _owner;
        private TeamType _ownerTeamType;
        private Vector3 _direction;
        private bool _isMovementStopped;
            
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
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
            
            EnableBullet(false);
            
            FactoryManager.Instance.ReturnObject(bulletData.poolableType, this);
        }

        public void Fire(Vector3 direction, Vector3 velocity)
        {
            _rigidbody.velocity += velocity;
            
            _direction = direction * bulletData.bulletForce;
            _rigidbody.AddTorque(transform.right, ForceMode.Impulse);
        }

        private void Bounce(Vector3 direction, float force)
        {
            const int errorAngle = 2; 

            var randomX = Random.Range(-errorAngle, errorAngle);
            var randomY = Random.Range(-errorAngle, errorAngle);
            
            var spreadRotation = Quaternion.Euler(randomX, randomY, 0);
            var deviatedDirection = spreadRotation * direction * force;
            
            _direction = deviatedDirection;
            _rigidbody.AddTorque(transform.right, ForceMode.Impulse);
        }

        public void SetOwner(Entity owner)
        {
            _owner = owner;
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

            var hitEntity = hitCollider.GetComponentInParent<Entity>();
            if (hitEntity && hitEntity != _owner)
            {
                if (hitEntity.GetAttributesComponent().IsShielded())
                {
                    SetOwner(hitEntity);
                    Bounce(-_direction.normalized, bulletData.bulletForce / 2f);
                    ApplyHit(collision, hitEntity);

                    return;
                }

                if (hitEntity.GetTeam() != _ownerTeamType)
                    transform.SetParent(hitEntity.transform);

                ApplyHit(collision, hitEntity);
            }

            if (hitEntity == _owner) return;

            EnableBullet(false);

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
            _rigidbody.isKinematic = !enable;
            _isMovementStopped = !enable;
            _collider.enabled = enable;
        }

        public void PauseEntity(bool pause) => EnableBullet(!pause);
        
        private void OnDestroy()
        {
            EventManager.GameEvents.Pause -= PauseEntity;
        }
    }
}