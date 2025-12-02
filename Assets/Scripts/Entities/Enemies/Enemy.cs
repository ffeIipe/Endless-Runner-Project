using Components;
using Entities.MVC;
using FiniteStateMachine;
using Managers;
using Pool;
using Scriptables;
using Scriptables.Entities;
using UnityEngine;

namespace Entities.Enemies
{
    //TODO: fix no muerte
    public abstract class Enemy : Entity, IPoolable
    {
        public Entity Owner { get; set; }
        
        private EnemyData EnemyData => (EnemyData)entityData;
        private StateMachine _stateMachine;
        private VisionComponent _visionComponent;
        
        public VisionComponent GetVisionComponent() => _visionComponent;
        protected StateMachine GetStateMachine() => _stateMachine;
        public EntityData GetData() => EnemyData;
        
        protected override void Awake()
        {
            base.Awake();
            
            _visionComponent = new VisionComponent(this, EnemyData, StartCoroutine, StopCoroutine);
            _stateMachine = new StateMachine(this);
        }

        protected override ViewBase InitializeView()
        {
            return new ViewEnemy(this, EnemyData, StartCoroutine);
        }

        public override void GetHit(Vector3 direction, Vector3 hitPoint, Vector3 hitNormal, float force)
        {
            base.GetHit(direction, hitPoint, hitNormal, force);

            View.ApplyDamageEffect(direction, hitPoint, hitNormal, force);
        }

        protected override void Die()
        {
            if (!CanTakeDamage) return;
            
            base.Die();

            GetStateMachine().Enabled = false;
            
            StartCoroutine(FactoryManager.Instance.ReturnObjectWithLifeTime(
                EnemyData.poolableType,
                this,
                5f
            ));
            
            if (LastDamageCauser && LastDamageCauser.TryGetComponent(out Player.Player player))
            {
                EventManager.PlayerEvents.OnEnemyKilled.Invoke();   
            }
        }

        protected override void OnLevelRestarted()
        {
            base.OnLevelRestarted();
            
            FactoryManager.Instance.ReturnObject(EnemyData.poolableType, this);
        }

        public virtual void Activate()
        {
            gameObject.SetActive(true);
            
            GetRigidbody().isKinematic = false;
            
            _visionComponent.EnableVision();

            GetStateMachine().Enabled = true;
            GetStateMachine().ChangeState("Idle");
            
            CanTakeDamage = true;
        }

        public virtual void Deactivate()
        {
            _visionComponent?.DisableVision();
            gameObject.SetActive(false);
            
            transform.rotation = Quaternion.identity;
            
            GetStateMachine().Enabled = false;
            ResetRigidBody();
        }

        private void ResetRigidBody()
        {
            GetRigidbody().isKinematic = false;
            GetRigidbody().velocity = Vector3.zero;
            GetRigidbody().angularVelocity = Vector3.zero;
            GetRigidbody().constraints = RigidbodyConstraints.FreezeRotation;
            GetRigidbody().isKinematic = true;
        }

        public override void PauseEntity(bool pause)
        {
            base.PauseEntity(pause);
            
            if (pause)
            {
                GetStateMachine().Enabled = false;
            }
            else
            {
                if (GetAttributesComponent().IsAlive())
                    GetStateMachine().Enabled = true;
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if(entityData != null) 
                Gizmos.DrawWireSphere(transform.position, EnemyData.attackDistance);
        }
    }
}