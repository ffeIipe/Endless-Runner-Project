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
        
        protected Entity LastDamageCauser;
        protected ViewBase View;
        protected RigidbodyConstraints SavedRigidbodyConstraints;
        [SerializeField]protected bool CanTakeDamage = true;
        
        private Rigidbody _rigidbody;
        private Vector3 _currentVelocity;
        private Vector3 _currentAngularVelocity;
        
        private AttributesComponent _attributesComponent;
        private TeamComponent _teamComponent;

        public AttributesComponent GetAttributesComponent() => _attributesComponent;
        public TeamType GetTeam() => _teamComponent.GetCurrentTeam();
        public Rigidbody GetRigidbody() => _rigidbody;
        public Entity GetLastDamageCauser() => LastDamageCauser;
        
        protected virtual void Awake()
        {
            View = InitializeView();
            
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            SavedRigidbodyConstraints = _rigidbody.constraints;
            
            _attributesComponent = new AttributesComponent(entityData.health, entityData.shield);
            _teamComponent = new TeamComponent(entityData.teamType);
        }

        protected virtual void OnEnable()
        {
            EventManager.GameEvents.Pause += PauseEntity;
            EventManager.GameEvents.OnLevelRestarted += OnLevelRestarted;
            EventManager.GameEvents.OnLevelChanged += OnLevelRestarted;
            
            _attributesComponent.OnDead += Die;
        }

        protected virtual void OnDisable()
        {
            EventManager.GameEvents.Pause -= PauseEntity;
            EventManager.GameEvents.OnLevelRestarted -= OnLevelRestarted;
            EventManager.GameEvents.OnLevelChanged -= OnLevelRestarted;
            
            _attributesComponent.OnDead -= Die;
        }

        protected virtual void Die()
        {
            View.OnEntityDead();
            CanTakeDamage = false;
        }

        protected virtual void OnLevelRestarted()
        {
            View.RestartEntityView();
            CanTakeDamage = true;
            _attributesComponent.Reset();
        }

        protected virtual ViewBase InitializeView()
        {
            return new ViewBase(this);
        }
        
        public virtual void TakeDamage(float damage, Entity damageCauser)
        {
            if(!CanTakeDamage) return;
                
            _attributesComponent.ReceiveDamage(damage);
            LastDamageCauser = damageCauser;
        }

        public virtual void GetHit(Vector3 direction, Vector3 hitPoint, Vector3 hitNormal, float force)
        {
            if (!_attributesComponent.IsAlive())
            {
                _rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
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
            if (pause)
            {
                _currentAngularVelocity = _rigidbody.angularVelocity;
                _currentVelocity = _rigidbody.velocity;
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                if(_rigidbody.isKinematic) return;
                
                _rigidbody.constraints = SavedRigidbodyConstraints;
                _rigidbody.angularVelocity = _currentAngularVelocity;
                _rigidbody.velocity = _currentVelocity;
            }
        }
    }
}