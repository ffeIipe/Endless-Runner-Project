using System;
using System.Collections;
using Enums;
using Managers;
using Scriptables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.MVC
{
    public class ViewEnemy : ViewBase
    {
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;
        private readonly Material _dissolveMaterial;
        private readonly VikingHelmet _vikingHelmet;
        private readonly EnemyData _enemyData;
        
        public ViewEnemy(Entity owner, EnemyData enemyData, Func<IEnumerator, Coroutine> startCoroutine) : base(owner)
        {
            _startCoroutine = startCoroutine;
            _enemyData = enemyData;
            
            _dissolveMaterial = owner.GetComponentInChildren<MeshRenderer>().material;
            _vikingHelmet = owner.GetComponentInChildren<VikingHelmet>();
            
            owner.OnDeactivated += OnEntityDeactivated;
        }

        public override void OnEntityDead()
        {
            base.OnEntityDead();
            
            ApplyDissolveEffect();
            
            EventManager.PlayerEvents.OnEnemyKilled.Invoke();
        }
        
        private void OnEntityDeactivated()
        {
            _vikingHelmet.ResetDeathProp();
            RemoveDissolveEffect();
        }

        public override void ApplyDamageEffect(Vector3 hitPoint, Vector3 hitNormal, float force)
        {
            base.ApplyDamageEffect(hitPoint, hitNormal, force);
            
            if (!Owner.GetAttributesComponent().IsAlive())
            {
                _vikingHelmet.ActivateDeathProp(hitPoint * force);
            }
        }

        public override void HeadShotEffect()
        {
            base.HeadShotEffect();
            //var chance = Random.Range(0, );
            if (true)
            {
                EffectsManager.Instance.PlayEffect(HitStopType.Fast);
            }
        }

        public override void OnShieldDamaged()
        {
            base.OnShieldDamaged();
            
            
        }

        private IEnumerator DissolveEffect()
        {
            var time = 0f;
    
            _vikingHelmet.ActivateDeathProp(Owner.GetRigidbody().velocity);
            _dissolveMaterial.SetFloat("_DissolveAmount", 0f);

            while (time < _enemyData.dissolveEffectDuration)
            {
                if (!GameManager.IsPaused)
                {
                    time += Time.fixedDeltaTime;
                    
                    var t = Mathf.Clamp01(time / _enemyData.dissolveEffectDuration);
                    var value = Mathf.Lerp(0f, 1f, t);
            
                    _dissolveMaterial.SetFloat("_DissolveAmount", value);
                }

                yield return null;
            }

            _dissolveMaterial.SetFloat("_DissolveAmount", 1f);
            yield return new WaitForSeconds(_enemyData.timeUntilDeactivation);
            
            OnEntityDeactivated();
        }

        private void ApplyDissolveEffect() => _startCoroutine(DissolveEffect());
        
        private void RemoveDissolveEffect() => _dissolveMaterial.SetFloat("_DissolveAmount", 0f);
    }
}