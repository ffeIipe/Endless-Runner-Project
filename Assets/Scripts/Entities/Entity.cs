using System;
using Components;
using Entities.MVC;
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
        
        public Action OnDeactivated =  delegate { };
        
        protected ViewBase View;
        protected RigidbodyConstraints SavedRigidbodyConstraints;
            
        private Rigidbody _rigidbody;
        private Vector3 _currentVelocity;
        private Vector3 _currentAngularVelocity;
        
        private AttributesComponent _attributesComponent;
        private TeamComponent _teamComponent;

        public AttributesComponent GetAttributesComponent() => _attributesComponent;
        public TeamType GetTeam() => _teamComponent.GetCurrentTeam();
        public Rigidbody GetRigidbody() => _rigidbody;
        
        protected virtual void Awake()
        {
            View = CreateView();
            
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            SavedRigidbodyConstraints = _rigidbody.constraints;
            
            _attributesComponent = new AttributesComponent(entityData.health, entityData.shield);
            _teamComponent = new TeamComponent(entityData.teamType);
        }

        protected virtual void OnEnable()
        {
            EventManager.GameEvents.Pause += PauseEntity;
            
            _attributesComponent.OnDead += View.OnEntityDead;
            _attributesComponent.OnShieldDamage += View.OnShieldDamaged;
        }

        protected virtual ViewBase CreateView()
        {
            return new ViewBase(this);
        }
        
        public void TakeDamage(float damage)
        {
            _attributesComponent.ReceiveDamage(damage);
        }

        public virtual void GetHit(Vector3 hitPoint, Vector3 hitNormal, float force)
        {
            if (!GetAttributesComponent().IsAlive())
            {
                _rigidbody.AddForce(hitPoint.normalized * force, ForceMode.Impulse);
            }

            if (!TryGetComponent(out CapsuleCollider coll)) return;
            
            var bounds = coll.bounds; 
            var totalHeight = bounds.size.y;
            var headThreshold = bounds.min.y + (totalHeight * 0.66f);

            if (hitPoint.y >= headThreshold)
            {
                View.HeadShotEffect();
            }
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