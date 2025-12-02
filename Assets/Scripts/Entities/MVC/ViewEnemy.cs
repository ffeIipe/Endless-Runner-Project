using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Managers;
using Scriptables.Entities;
using UnityEngine;

namespace Entities.MVC
{
    public class ViewEnemy : ViewBase
    {
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;
        private readonly Material _dissolveMaterial;
        private readonly int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");
        private readonly EnemyData _enemyData;
        private readonly Dictionary<string, DetachableProp> _detachableProps;
        
        public ViewEnemy(Entity owner, EnemyData enemyData, Func<IEnumerator, Coroutine> startCoroutine) : base(owner)
        {
            _startCoroutine = startCoroutine;
            _enemyData = enemyData;
            
            _dissolveMaterial = owner.GetComponentInChildren<MeshRenderer>().material;

            _detachableProps = new Dictionary<string, DetachableProp>();
            foreach (var prop in owner.GetComponentsInChildren<DetachableProp>())
            {
                _detachableProps.Add(prop.propName, prop);
            }
        }

        public override void OnEntityDead()
        {
            base.OnEntityDead();
            
            ApplyDissolveEffect();
        }

        public override void ApplyDamageEffect(Vector3 direction, Vector3 hitPoint, Vector3 hitNormal, float force)
        {
            base.ApplyDamageEffect(direction, hitPoint, hitNormal, force);
            
            if (!Owner.GetAttributesComponent().IsAlive())
            {
                if (_detachableProps.TryGetValue("Helmet", out var helmet))
                    helmet.ActivatePhysicsProp(direction * force);
            }

            if (!Owner.GetAttributesComponent().IsShielded())
            {
                if(_detachableProps.TryGetValue("Shield", out var shield))
                    shield.ActivatePhysicsProp(direction * force);
            }
        }

        public override void HeadShotEffect()
        {
            base.HeadShotEffect();
            
            EffectsManager.Instance.PlayEffect(HitStopType.Fast);
        }

        public override void RestartEntityView()
        {
           foreach (var prop in _detachableProps)
           {
               prop.Value.ResetPhysicsProp();
           }
           RemoveDissolveEffect();
        }

        private IEnumerator DissolveEffect()
        {
            var time = 0f;
    
            _dissolveMaterial.SetFloat(_dissolveAmount, 0f);

            while (time < _enemyData.dissolveEffectDuration)
            {
                if (!GameManager.IsPaused)
                {
                    time += Time.fixedDeltaTime;
                    
                    var t = Mathf.Clamp01(time / _enemyData.dissolveEffectDuration);
                    var value = Mathf.Lerp(0f, 1f, t);
            
                    _dissolveMaterial.SetFloat(_dissolveAmount, value);
                }

                yield return null;
            }

            _dissolveMaterial.SetFloat(_dissolveAmount, 1f);
            yield return new WaitForSeconds(_enemyData.timeUntilDeactivation);

            RestartEntityView();
        }

        private void ApplyDissolveEffect() => _startCoroutine(DissolveEffect());
        
        private void RemoveDissolveEffect() => _dissolveMaterial.SetFloat(_dissolveAmount, 0f);
    }
}