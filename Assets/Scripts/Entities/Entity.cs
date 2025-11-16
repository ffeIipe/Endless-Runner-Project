using Components;
using Enums;
using Interfaces;
using Scriptables;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour, IHittable, ITeammate
    {
        public EntityData entityData;
        public Transform handPoint;
        
        private AttributesComponent _attributesComponent;
        private TeamComponent _teamComponent;
        
        protected virtual void Awake()
        {
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
            Debug.Log("Dead");
        }
        
        public AttributesComponent GetAttributesComponent() => _attributesComponent;
        public Team GetTeam() => _teamComponent.GetCurrentTeam();
    }
}