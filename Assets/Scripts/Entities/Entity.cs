using Components;
using Enums;
using Interfaces;
using Managers;
using Scriptables;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entity : MonoBehaviour, IHittable, ITeammate, IPausable
    {
        public EntityData entityData;
        public Transform handPoint;
        
        protected RigidbodyConstraints SavedRigidbodyConstraints;
            
        private Rigidbody _rigidbody;
        private Vector3 _currentVelocity;
        
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
            
            EventManager.Instance.gameEvents.Pause += PauseEntity;
        }
        
        public void TakeDamage(float damage)
        {
            _attributesComponent.ReceiveDamage(damage);
        }

        protected virtual void Dead()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.constraints = RigidbodyConstraints.None;
        }

        public virtual void PauseEntity(bool pause)
        {
            if (pause)
            {
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                _rigidbody.constraints = SavedRigidbodyConstraints;
            }
        }
    }
}