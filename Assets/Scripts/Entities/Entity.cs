using Components;
using Interfaces;
using Scriptables;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour, IHittable
    {
        public EntityData entityData;
        private Rigidbody _rigidbody;
        private AttributesComponent _attributesComponent;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _attributesComponent = new AttributesComponent(entityData.health, 0f);
            _attributesComponent.OnDead += Dead;
        }
        public void TakeDamage(float damage)
        {
            _attributesComponent.ReceiveDamage(damage);
        }

        protected virtual void Dead()
        {
            Debug.Log("Dead");
        }
        
        public Rigidbody GetRigidbody() => _rigidbody;
        protected AttributesComponent AttributesComponent => _attributesComponent;
    }
}