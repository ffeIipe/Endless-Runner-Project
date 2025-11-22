using Components;
using Entities.MVC;
using FiniteStateMachine;
using Managers;
using Pool;
using Scriptables;
using UnityEngine;

namespace Entities.Enemies
{
    public abstract class Enemy : Entity, IPoolable
    {
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

        protected override ViewBase CreateView()
        {
            return new ViewEnemy(this, EnemyData, StartCoroutine);
        }

        public override void GetHit(Vector3 hitPoint, Vector3 hitNormal, float force)
        {
            base.GetHit(hitPoint, hitNormal, force);

            View.ApplyDamageEffect(hitPoint, hitNormal, force);
        }
        
        public virtual void Activate()
        {
            GetRigidbody().constraints = SavedRigidbodyConstraints;
            gameObject.SetActive(true);
            
            _visionComponent.EnableVision();

            GetStateMachine().ChangeState("Idle");
            GetStateMachine().Enabled = true;
        }

        public virtual void Deactivate()
        {
            _visionComponent.DisableVision();
            gameObject.SetActive(false);
            
            OnDeactivated?.Invoke();
            FactoryManager.Instance.ReturnObject(EnemyData.poolableType, this);
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
                if(GetAttributesComponent().IsAlive())
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