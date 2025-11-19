using System.Collections;
using Components;
using FiniteStateMachine;
using Managers;
using Pool;
using Scriptables;
using UnityEngine;

namespace Entities.Enemies
{
    public abstract class Enemy : Entity, IPoolable
    {
        protected CountdownTimer DeadTimer;
        
        private EnemyData EnemyData => (EnemyData)entityData;
        private FSM _fsm;
        private VisionComponent _visionComponent;
        private Material _dissolveMaterial;
        
        public VisionComponent GetVisionComponent() => _visionComponent;
        protected FSM GetFSM() => _fsm;
        public EntityData GetData() => EnemyData;
        
        protected override void Awake()
        {
            base.Awake();
            
            DeadTimer = new CountdownTimer(2f);
            DeadTimer.OnTimerStop += ApplyDissolveEffect;
            
            GetAttributesComponent().OnDead += () => DeadTimer.Start();
            
            _visionComponent = new VisionComponent(this, EnemyData, StartCoroutine);
            _fsm = new FSM(this);
            _dissolveMaterial = GetComponentInChildren<MeshRenderer>().material;
        }

        private void FixedUpdate()
        {
            DeadTimer.Tick(Time.fixedDeltaTime);
        }

        protected override void Dead()
        {
            base.Dead();
            
            GetFSM().Enabled  = false;
        }
        
        public virtual void Activate()
        {
            GetRigidbody().constraints = SavedRigidbodyConstraints;
            gameObject.SetActive(true);
            
            GetFSM().ChangeState("Idle");
            GetFSM().Enabled = true;
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
            RemoveDissolveEffect();
        }
        
        public override void PauseEntity(bool pause)
        {
            base.PauseEntity(pause);
            
            if (pause)
            {
                GetFSM().Enabled = false;
            }
            else
            {
                GetFSM().Enabled = true;
            }
        }
        
        private IEnumerator DissolveEffect()
        {
            var time = 0f;
    
            _dissolveMaterial.SetFloat("_DissolveAmount", 0f);

            while (time < EnemyData.dissolveEffectDuration)
            {
                if (!GameManager.IsPaused)
                {
                    time += Time.fixedDeltaTime;
            
                    var t = Mathf.Clamp01(time / EnemyData.dissolveEffectDuration);
                    var value = Mathf.Lerp(0f, 1f, t);
            
                    _dissolveMaterial.SetFloat("_DissolveAmount", value);
                }

                yield return null;
            }

            _dissolveMaterial.SetFloat("_DissolveAmount", 1f);
            yield return new WaitForSeconds(EnemyData.timeUntilDeactivation);
            
            //OnDissolveFinished? += Deactivate
            Deactivate();
        }

        private void ApplyDissolveEffect() => StartCoroutine(DissolveEffect());
        private void RemoveDissolveEffect() => _dissolveMaterial.SetFloat("_DissolveAmount", 0f);
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, EnemyData.attackDistance);
        }
    }
}