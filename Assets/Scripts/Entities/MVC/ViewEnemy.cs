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
        private readonly int _fresnelColor = Shader.PropertyToID("_FresnelColor");
        private readonly int _fresnelPower = Shader.PropertyToID("_FresnelPower");
        private const float MaxFresnel = 4.5f;
        private const float TargetFresnel = 1f;
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

        public void AttackEffect()
        {
            _startCoroutine(AttackEffectCoroutine(_enemyData.attackCooldown, Color.red));
        }
        
        public void NotAttackEffect()
        {
            _startCoroutine(AttackEffectCoroutine(_enemyData.attackCooldown / 2, Color.white));
        }

        private IEnumerator AttackEffectCoroutine(float dur, Color colorTarget)
        {
            var time = 0f;

            while (time < dur)
            {
                time += Time.deltaTime;

                var t = time / dur;
                
                _dissolveMaterial.SetFloat(_fresnelPower, Mathf.Lerp(0, 1f, t));
                
                var newColor = Color.Lerp(_dissolveMaterial.GetColor(_fresnelColor), colorTarget, t);
                _dissolveMaterial.SetColor(_fresnelColor, newColor);
                
                yield return null;
            }
            
            _dissolveMaterial.SetColor(_fresnelColor, colorTarget);
        }
    }
}