using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using ScreenManagerFolder;
using Scriptables;
using Structs;
using UnityEngine;

namespace Managers
{
    public class EffectsManager : MonoBehaviour
    {
        public static EffectsManager Instance { get; private set; }
        [SerializeField] private EffectsManagerData effectsManagerData;    
        
        public Camera playerCamera;
        
        private Dictionary<HitStopType, HitStopMap> _hitStopMaps;
        private Dictionary<TimeWarpType, TimeWarpMap> _timeWarpMaps;
        private Dictionary<FieldOfViewWarpType, FieldOfViewMap> _fieldOfViewMaps;
        
        private float _targetFieldOfView;
        private float _targetVignetteIntensity;
		
		private float _maxPlayerHealth;
		
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _hitStopMaps = new Dictionary<HitStopType, HitStopMap>();
            foreach (var hitStopMap in effectsManagerData.hitStopMap)
            {
                _hitStopMaps.Add(hitStopMap.hitStopType, hitStopMap);
            }

            _timeWarpMaps =  new Dictionary<TimeWarpType, TimeWarpMap>();
            foreach (var timeWarpMap in effectsManagerData.timeWarpMap)
            {
                _timeWarpMaps.Add(timeWarpMap.timeWarpType, timeWarpMap);
            }

            _fieldOfViewMaps = new Dictionary<FieldOfViewWarpType, FieldOfViewMap>();
            foreach (var fieldOfViewMap in effectsManagerData.fieldOfViewMap)
            {
                _fieldOfViewMaps.Add(fieldOfViewMap.fieldOfViewWarpType, fieldOfViewMap);
            }

            ResetEffects();
        }

        private void OnEnable()
        {
            EventManager.GameEvents.OnLevelRestarted += ResetEffects;
        }

        private void OnDisable()
        {
            EventManager.GameEvents.OnLevelRestarted -= ResetEffects;
        }

        public void ResetEffects()
        {
            effectsManagerData.bloodRenderMaterial.SetFloat(effectsManagerData.vignetteIntensity, 0f);
            effectsManagerData.windRenderMaterial.SetFloat(effectsManagerData.vignetteIntensity, 0f);
            effectsManagerData.berserkerRenderMaterial.SetFloat(effectsManagerData.vignetteIntensity, 0f);
            effectsManagerData.healthRenderMaterial.SetFloat(effectsManagerData.vignetteIntensity, 0f);
            effectsManagerData.shieldRenderMaterial.SetFloat(effectsManagerData.vignetteIntensity, 0f);
        }

        private void Start()
        {
            playerCamera = Camera.main;
        }

        public void PlayEffect(HitStopType hitStopType) => StartCoroutine(DoHitStop(hitStopType));
        public void PlayEffect(TimeWarpType timeWarpType, Action onFinishedTimeWarp = null) => StartCoroutine(DoTimeWarp(timeWarpType, onFinishedTimeWarp));
        public void PlayFadeScreen(bool isFadeIn)
        {
            StartCoroutine(isFadeIn ? FadeInScreen() : FadeOutScreen());
        }

        public void UpdateVelocityEffect(float vel)
        {
            var t = Mathf.Clamp01(vel / 22f); //max. velocity of the player

            var minFOV = _fieldOfViewMaps[FieldOfViewWarpType.Type1].fieldOfViewWarpAttributes.minFOV;
            var maxFOV = _fieldOfViewMaps[FieldOfViewWarpType.Type1].fieldOfViewWarpAttributes.maxFOV;
            _targetFieldOfView = Mathf.Lerp(minFOV, maxFOV, t);

            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, _targetFieldOfView, Time.deltaTime);

            var currentVignette = effectsManagerData.windRenderMaterial.GetFloat(effectsManagerData.vignetteIntensity);
            var finalVignette = Mathf.Lerp(currentVignette, t, Time.deltaTime * effectsManagerData.windLerpSpeed);
            effectsManagerData.windRenderMaterial.SetFloat(effectsManagerData.vignetteIntensity, finalVignette); 
        }

        public void BloodEffect(float currentHealth)
        {
            var healthPercent = Mathf.Clamp01(currentHealth / 3f); //max life
            var damagePercent = 1f - healthPercent;

            StartCoroutine(UpdateRenderMaterialEffect(
                damagePercent, 
                effectsManagerData.bloodRenderMaterial,
                effectsManagerData.bloodLerpSpeed,
                false
            ));
        }

        public void BerserkerEffect(bool active)
        {
            var f = active ? 1f : 0f;
            
            StartCoroutine(UpdateRenderMaterialEffect(
                f, 
                effectsManagerData.berserkerRenderMaterial,
                effectsManagerData.berserkerLerpSpeed,
                false
            ));
        }

        public void HealthEffect()
        {
            StartCoroutine(UpdateRenderMaterialEffect(
                1f, 
                effectsManagerData.healthRenderMaterial,
                effectsManagerData.healthLerpSpeed,
                true
            ));
        }
        
        public void ShieldEffect(bool active)
        {
            var f = active ? 1f : 0f;
            
            StartCoroutine(UpdateRenderMaterialEffect(
                f, 
                effectsManagerData.shieldRenderMaterial,
                effectsManagerData.shieldLerpSpeed,
                false
            ));
        }

        private IEnumerator UpdateRenderMaterialEffect(float target, Material material, float duration, bool shouldReverse)
        {
            var start = material.GetFloat(effectsManagerData.vignetteIntensity);
            var time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                var t = time / duration;
        
                var currentVal = Mathf.Lerp(start, target, t);
        
                material.SetFloat(effectsManagerData.vignetteIntensity, currentVal);
                yield return null;
            }

            material.SetFloat(effectsManagerData.vignetteIntensity, target);

            if (shouldReverse)
            {
                StartCoroutine(UpdateRenderMaterialEffect(0f, material, duration, false));
            }
        }
        
        private IEnumerator FadeOutScreen()
        {
            var duration = effectsManagerData.fadeDuration; 
            var curve = effectsManagerData.fadeCurve; 
    
            var fadeScreen = (FadeScreen)ScreenManager.Instance.GetScreen(ScreenType.FadeScreen);
            ScreenManager.Instance.PushScreen(ScreenType.FadeScreen, false);
    
            var color = new Color(0, 0, 0, 0);
            fadeScreen.fadeImage.color = color;
    
            var time = 0f; 
    
            while (time < duration)
            {
                time += Time.deltaTime; 
        
                var normalizedTime = time / duration; 
                var curveValue = curve.Evaluate(normalizedTime); 
        
                color.a = curveValue;
                fadeScreen.fadeImage.color = color;
        
                yield return null;
            }

            color.a = 1f;
            fadeScreen.fadeImage.color = color;
        }

        private IEnumerator FadeInScreen()
        {
            var duration  = effectsManagerData.fadeDuration;
            var curve = effectsManagerData.fadeCurve;
            
            var fadeScreen = (FadeScreen)ScreenManager.Instance.GetScreen(ScreenType.FadeScreen);
            ScreenManager.Instance.PushScreen(ScreenType.FadeScreen, true);
            
            var currentColor = fadeScreen.fadeImage.color;
            
            var time = 0f;
            
            while (time < duration)
            {
                time += Time.deltaTime;
                
                var normalizedTime = time / duration;
                var curveValue = curve.Evaluate(normalizedTime);
                
                currentColor.a = -curveValue;
                fadeScreen.fadeImage.color = currentColor;
                
                yield return null;
            }
            
            currentColor.a = 0f;
            fadeScreen.fadeImage.color = currentColor;
        }

        private IEnumerator DoHitStop(HitStopType hitStopType)
        {
            if (!_hitStopMaps.TryGetValue(hitStopType, out var hitStopMap)) 
            {
                yield break;
            }

            var curve = hitStopMap.hitStopAttributes.curve;
            var attr = hitStopMap.hitStopAttributes;
            
            if (curve.length == 0) yield break;

            var duration = attr.duration;
            var timer = 0f;

            while (timer < duration)
            {
                var normalizedTime = timer / duration;
                
                var curveValue = curve.Evaluate(normalizedTime);
                Time.timeScale = curveValue * attr.minScale;
                timer += Time.unscaledDeltaTime;
                
                yield return null;
            }

            Time.timeScale = 1f; 
        }

        private IEnumerator DoTimeWarp(TimeWarpType timeWarpType, Action onFinishedTimeWarp)
        {
            if (!_timeWarpMaps.TryGetValue(timeWarpType, out var timeWarpMap))
            {
                yield break;
            }
            
            var attr = timeWarpMap.timeWarpAttributes;
            
            var curve = attr.curve;
            var duration = attr.duration;
            var timer = 0f;

            while (timer < duration)
            {
                var normalizedTime = timer / duration;
                var curveValue = curve.Evaluate(normalizedTime);
                
                Time.timeScale = curveValue;
                timer += Time.unscaledDeltaTime;
                
                yield return null;
            }

            Time.timeScale = 1;
            onFinishedTimeWarp?.Invoke();
        }
    }
}