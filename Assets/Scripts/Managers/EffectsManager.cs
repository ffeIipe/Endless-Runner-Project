using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using ScreenManagerFolder;
using Scriptables;
using Structs;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Managers
{
    public class EffectsManager : MonoBehaviour
    {
        public static EffectsManager Instance { get; private set; }
        [SerializeField] private EffectsManagerData effectsManagerData;    
        
        private Camera _camera;
        
        private Dictionary<HitStopType, HitStopMap> _hitStopMaps;
        private Dictionary<TimeWarpType, TimeWarpMap> _timeWarpMaps;
        private Dictionary<FieldOfViewWarpType, FieldOfViewMap> _fieldOfViewMaps;
        
        private float _targetFieldOfView;
        private float _targetVignetteIntensity;
        
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

        private void ResetEffects()
        {
            effectsManagerData.bloodRenderMaterial.SetFloat(effectsManagerData.vignetteIntensity, 0f);
            effectsManagerData.windRenderMaterial.SetFloat(effectsManagerData.vignetteIntensity, 0f);
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        public void PlayEffect(HitStopType hitStopType) => StartCoroutine(DoHitStop(hitStopType));
        public void PlayEffect(TimeWarpType timeWarpType, Action onFinishedTimeWarp) => StartCoroutine(DoTimeWarp(timeWarpType, onFinishedTimeWarp));
        public void PlayFadeScreen(bool isFadeIn)
        {
            StartCoroutine(isFadeIn ? FadeInScreen() : FadeOutScreen());
        }

        public void UpdateVelocityEffect(float vel)
        {
            if(!_camera) _camera = Camera.main; //TODO: fixear race condition!
            
            var t = Mathf.Clamp01(vel / 22f); // 22 es la vel max del player

            var minFOV = _fieldOfViewMaps[FieldOfViewWarpType.Type1].fieldOfViewWarpAttributes.minFOV;
            var maxFOV = _fieldOfViewMaps[FieldOfViewWarpType.Type1].fieldOfViewWarpAttributes.maxFOV;
            _targetFieldOfView = Mathf.Lerp(minFOV, maxFOV, t);

            var vignetteT = t < 0.1 ? 0f : t;
            _targetVignetteIntensity = vignetteT * 1.5f; // multiplier hardcodeado tmb jeje

            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFieldOfView, Time.deltaTime * effectsManagerData.windLerpSpeed);

            var currentVignette = effectsManagerData.windRenderMaterial.GetFloat(effectsManagerData.vignetteIntensity);
            var finalVignette = Mathf.Lerp(currentVignette, _targetVignetteIntensity, Time.deltaTime * effectsManagerData.windLerpSpeed);
            effectsManagerData.windRenderMaterial.SetFloat(effectsManagerData.vignetteIntensity, finalVignette); 
        }

        public void BloodEffect(float currentHealth)
        {
            var healthPercent = Mathf.Clamp01(currentHealth / 3f); //max life
            var damagePercent = 1f - healthPercent;
            var targetIntensity = damagePercent * 2f; //max vignette intensity

            StartCoroutine(UpdateBloodEffect(targetIntensity));
        }

        private IEnumerator UpdateBloodEffect(float target)
        {
            var bloodMat = effectsManagerData.bloodRenderMaterial;
            var start = bloodMat.GetFloat(effectsManagerData.vignetteIntensity);
            var time = 0f;
            var duration = effectsManagerData.bloodLerpSpeed;

            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                var t = time / duration;
        
                var currentVal = Mathf.Lerp(start, target, t);
        
                bloodMat.SetFloat(effectsManagerData.vignetteIntensity, currentVal);
                yield return null;
            }

            bloodMat.SetFloat(effectsManagerData.vignetteIntensity, target);
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

            onFinishedTimeWarp?.Invoke();
        }
    }
}