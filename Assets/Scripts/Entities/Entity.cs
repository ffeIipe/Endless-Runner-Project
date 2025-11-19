using Components;
using Enums;
using Interfaces;
using Managers;
using Scriptables;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entity : MonoBehaviour, ITeammate, IPausable, IHittable
    {
        public EntityData entityData;
        public Transform handPoint;
        
        protected RigidbodyConstraints SavedRigidbodyConstraints;
            
        private Rigidbody _rigidbody;
        private Vector3 _currentVelocity;
        private Vector3 _currentAngularVelocity;
        
        private AttributesComponent _attributesComponent;
        private TeamComponent _teamComponent;

        public AttributesComponent GetAttributesComponent() => _attributesComponent;
        public Team GetTeam() => _teamComponent.GetCurrentTeam();
        public Rigidbody GetRigidbody() => _rigidbody;
        
        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            SavedRigidbodyConstraints = _rigidbody.constraints;
            
            _attributesComponent = new AttributesComponent(entityData.health, 0f);
            _attributesComponent.OnDead += Dead;
            
            _teamComponent = new TeamComponent(entityData.team);
        }

        protected virtual void Start()
        {
            EventManager.GameEvents.Pause += PauseEntity;
        }
        
        public void TakeDamage(float damage)
        {
            _attributesComponent.ReceiveDamage(damage);
        }

        public virtual void GetHit(Vector3 hitPoint, float force)
        {
            if (!GetAttributesComponent().IsAlive())
            {
                _rigidbody.AddForce(hitPoint.normalized * force, ForceMode.Impulse);
            }
        }

        protected virtual void Dead()
        {
            _rigidbody.isKinematic = false;
            SavedRigidbodyConstraints = RigidbodyConstraints.None;
            _rigidbody.constraints = SavedRigidbodyConstraints;
        }

        public virtual void PauseEntity(bool pause)
        {
            if(!_rigidbody)
            {
                return;
            }
            
            if (pause)
            {
                _currentAngularVelocity = _rigidbody.angularVelocity;
                _currentVelocity =  _rigidbody.velocity;
                
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                _rigidbody.constraints = SavedRigidbodyConstraints;
                
                _rigidbody.angularVelocity = _currentAngularVelocity;
                _rigidbody.velocity = _currentVelocity;
            }
        }
        
        protected virtual void OnDestroy()
        {
            EventManager.GameEvents.Pause -= PauseEntity;
        }
    }
}