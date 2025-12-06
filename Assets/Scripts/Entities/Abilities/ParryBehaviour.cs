using System.Collections;
using Enums;
using Managers;
using Scriptables.Abilities;
using UnityEngine;

namespace Entities.Abilities
{
    [RequireComponent(typeof(SphereCollider))]
    public class ParryBehaviour : Ability
    {   
        public ParryData ParryData => (ParryData)abilityData;
        
        private SphereCollider _collider;
        
        private void Awake()
        {
            if (TryGetComponent(out SphereCollider sphereCollider))
            {
                _collider = sphereCollider;
                _collider.isTrigger = true;
                _collider.enabled = false;
                _collider.radius = ParryData.radiusRange;
            }
        }

        public override void Activate()
        {
            gameObject.SetActive(true);
            _collider.enabled = true;

            var corr = FactoryManager.Instance.ReturnObjectWithLifeTime(
                ParryData.poolableType,
                this,
                ParryData.duration
            );
            
            StartCoroutine(corr);
        }

        public override void Deactivate()
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }

        public override void SetOwner(Entity owner)
        {
            base.SetOwner(owner);
            transform.SetParent(owner.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                bullet.Bounce(ParryData.parryForce, Owner);
                
                EffectsManager.Instance.PlayEffect(TimeWarpType.Fast);
                
                Deactivate();
            }
        }
    }
}