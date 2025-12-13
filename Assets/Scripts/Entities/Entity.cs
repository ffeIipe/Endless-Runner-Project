using Components;
using Entities.MVC;
using Enums;
using Interfaces;
using Managers;
using Managers.SoundManagerFolder;
using Scriptables;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Entity : MonoBehaviour, ITeammate, IPausable, IHittable
    {
        public EntityData entityData;
        public Transform handPoint;
        public AudioSource audioSource;
        
        protected Entity LastDamageCauser;
        protected ViewBase View;
        protected RigidbodyConstraints SavedRigidbodyConstraints;
        protected bool CanTakeDamage = true;
        
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
            
            audioSource = GetComponent<AudioSource>();
            
            _attributesComponent = new AttributesComponent(entityData.health, entityData.shield);
            _teamComponent = new TeamComponent(entityData.teamType);
        }

        protected virtual void OnEnable()
        {
            EventManager.GameEvents.Pause += PauseEntity;
            EventManager.GameEvents.OnLevelUpdated += OnLevelUpdated;
            
            _attributesComponent.OnDead += Die;
        }

        protected virtual void OnDisable()
        {
            EventManager.GameEvents.Pause -= PauseEntity;
            EventManager.GameEvents.OnLevelUpdated -= OnLevelUpdated;
            
            _attributesComponent.OnDead -= Die;
        }

        public virtual void Die()
        {
            View.OnEntityDead();
            CanTakeDamage = false;
        }

        protected virtual void OnLevelUpdated()
        {
            CanTakeDamage = true;
            _attributesComponent.Reset();
            View.RestartEntityView();
        }

        protected virtual ViewBase InitializeView()
        {
            return new ViewBase(this);
        }
        
        public virtual void TakeDamage(float damage, Entity damageCauser)
        {
            LastDamageCauser = damageCauser;
            
            if(!CanTakeDamage) return;
                
            _attributesComponent.ReceiveDamage(damage);
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
            
            SoundManager.Instance.PlaySound(SoundType.FleshImpact, audioSource);
            SoundManager.Instance.PlaySound(SoundType.Grunt, audioSource);
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