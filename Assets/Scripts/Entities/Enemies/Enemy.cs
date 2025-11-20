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
        private EnemyData EnemyData => (EnemyData)entityData;
        private FSM _fsm;
        private VisionComponent _visionComponent;
        private Material _dissolveMaterial;
        private VikingHelmet _vikingHelmet;
        
        public VisionComponent GetVisionComponent() => _visionComponent;
        protected FSM GetFSM() => _fsm;
        public EntityData GetData() => EnemyData;
        
        protected override void Awake()
        {
            base.Awake();
            
            GetAttributesComponent().OnDead += ApplyDissolveEffect;
            
            _visionComponent = new VisionComponent(this, EnemyData, StartCoroutine, StopCoroutine);
            _fsm = new FSM(this);
            _dissolveMaterial = GetComponentInChildren<MeshRenderer>().material;
            
            _vikingHelmet = GetComponentInChildren<VikingHelmet>();
        }

        public override void GetHit(Vector3 hitPoint, float force)
        {
            base.GetHit(hitPoint, force);

            if (!GetAttributesComponent().IsAlive())
            {
                _vikingHelmet.ActivateDeathProp(hitPoint * force);
            }
        }

        protected override void Dead()
        {
            base.Dead();
            _visionComponent.DisableVision();
            GetFSM().Enabled  = false;
        }
        
        public virtual void Activate()
        {
            GetRigidbody().constraints = SavedRigidbodyConstraints;
            gameObject.SetActive(true);
            
            _visionComponent.EnableVision();

            GetFSM().ChangeState("Idle");
            GetFSM().Enabled = true;
        }

        public virtual void Deactivate()
        {
            _visionComponent.DisableVision();
            gameObject.SetActive(false);
            
            _vikingHelmet.ResetDeathProp();
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
                if(GetAttributesComponent().IsAlive())
                    GetFSM().Enabled = true;
            }
        }
        
        private IEnumerator DissolveEffect()
        {
            var time = 0f;
    
            _vikingHelmet.ActivateDeathProp(GetRigidbody().velocity);
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
            
            Deactivate();
        }

        private void ApplyDissolveEffect() => StartCoroutine(DissolveEffect());
        private void RemoveDissolveEffect() => _dissolveMaterial.SetFloat("_DissolveAmount", 0f);
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if(entityData != null) 
                Gizmos.DrawWireSphere(transform.position, EnemyData.attackDistance);
        }
    }
}