using System.Collections;
using Components;
using FiniteStateMachine;
using Managers;
using Scriptables;
using UnityEngine;

namespace Entities.Enemies
{
    public abstract class Enemy : Entity
    {
        protected EnemyData EnemyData;
        private FSM _fsm;
        private VisionComponent _visionComponent;
        private Material _dissolveMaterial;
        
        public VisionComponent GetVisionComponent() => _visionComponent;
        protected FSM GetFSM() => _fsm;
        public EntityData GetData() => EnemyData;
        
        protected override void Awake()
        {
            base.Awake();
            
            EnemyData = (EnemyData)entityData;
            
            _visionComponent = new VisionComponent(this, EnemyData, StartCoroutine);
            _fsm = new FSM(this);
            _dissolveMaterial = GetComponentInChildren<MeshRenderer>().material;
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
            Debug.Log("Dissolve Effect");
            var time = 0f;
    
            _dissolveMaterial.SetFloat("_DissolveAmount", 0f);

            while (time < EnemyData.dissolveEffectDuration)
            {
                Debug.Log("Applying Dissolve Effect");
                if (!GameManager.isPaused)
                {
                    time += Time.deltaTime;
            
                    var t = Mathf.Clamp01(time / EnemyData.dissolveEffectDuration);
                    var value = Mathf.Lerp(0f, 1f, t);
            
                    _dissolveMaterial.SetFloat("_DissolveAmount", value);
                }

                yield return null;
            }

            _dissolveMaterial.SetFloat("_DissolveAmount", 1f);
            gameObject.SetActive(false);
        }
        protected void ApplyDissolveEffect() => StartCoroutine(DissolveEffect());
        protected void RemoveDissolveEffect() => _dissolveMaterial.SetFloat("_DissolveAmount", 0f);
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, EnemyData.attackDistance);
        }
    }
}