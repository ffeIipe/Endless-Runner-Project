using System;
using Enums;
using Interfaces;
using Pool;
using Scriptables;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private BulletData bulletData;
        private Rigidbody _rigidbody;
        private Entity _owner;
        private Team _ownerTeam;
        private Vector3 _direction;
        private bool _isMovementStopped;
            
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
            _isMovementStopped = false;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
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

            var team = _owner.GetComponent<ITeammate>();
            if (team != null)
            {
                _ownerTeam = team.GetTeam();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _isMovementStopped = true;
            _rigidbody.isKinematic = true;
            
            var entity = collision.collider.GetComponentInParent<Entity>();
            if (entity == null) return;

            if (entity.GetTeam() != _ownerTeam)
            {
                entity.TakeDamage(bulletData.damage);
            }
        }
    }
}