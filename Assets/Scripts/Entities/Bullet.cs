using System;
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
        private Team _ownerTeam;
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
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
            _isMovementStopped = false;
            
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            
            transform.parent = null;
            
            _rigidbody.isKinematic = true;
        }

        public void Fire(Vector3 direction, Quaternion rotation, Vector3 velocity)
        {
            _rigidbody.velocity += velocity;
            transform.rotation = rotation;
            
            _direction = direction * bulletData.bulletForce;
            _rigidbody.AddTorque(transform.right, ForceMode.Impulse);
        }

        public void SetOwner(Entity owner)
        {
            _owner = owner;
            _ownerTeam = owner.GetTeam();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var entity = collision.collider.GetComponentInParent<Entity>();
            if (entity == _owner) return;
            
            _isMovementStopped = true;
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            
            if (entity == null) return;

            if (entity.GetTeam() != _ownerTeam)
            {
                transform.SetParent(entity.transform);
                entity.TakeDamage(bulletData.damage);
                entity.GetHit(_direction.normalized, bulletData.bulletForce);
            }
        }

        public void PauseEntity(bool pause)
        {
            if (pause)
            {
                _rigidbody.isKinematic = true;
                _isMovementStopped = true;
            }
            else
            {
                _rigidbody.isKinematic = false;
                _isMovementStopped = false;
            }
        }

        private void OnDestroy()
        {
            EventManager.GameEvents.Pause -= PauseEntity;
        }
    }
}