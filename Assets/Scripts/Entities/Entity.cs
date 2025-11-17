using Components;
using Enums;
using Interfaces;
using Scriptables;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entity : MonoBehaviour, IHittable, ITeammate
    {
        public EntityData entityData;
        public Transform handPoint;
        
        private Rigidbody _rigidbody;
        
        private AttributesComponent _attributesComponent;
        private TeamComponent _teamComponent;
        
        public AttributesComponent GetAttributesComponent() => _attributesComponent;
        public Team GetTeam() => _teamComponent.GetCurrentTeam();
        public Rigidbody GetRigidbody() => _rigidbody;
        
        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            
            _attributesComponent = new AttributesComponent(entityData.health, 0f);
            _attributesComponent.OnDead += Dead;
            
            _teamComponent = new TeamComponent(entityData.team);
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
    }
}