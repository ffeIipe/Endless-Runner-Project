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
        
        private Camera _camera;
        
        private Dictionary<HitStopType, HitStopMap> _hitStopMaps;
        private Dictionary<TimeWarpType, TimeWarpMap> _timeWarpMaps;
        private Dictionary<FieldOfViewWarpType, FieldOfViewMap> _fieldOfViewMaps;
        
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
        }

        private void OnEnable()
        {
            _camera = Camera.main;
        }

        public void PlayEffect(HitStopType hitStopType) => StartCoroutine(DoHitStop(hitStopType));
        public void PlayEffect(TimeWarpType timeWarpType, Action onFinishedTimeWarp) => StartCoroutine(DoTimeWarp(timeWarpType, onFinishedTimeWarp));
        public void PlayFadeScreen(bool isFadeIn)
        {
            StartCoroutine(isFadeIn ? FadeInScreen() : FadeOutScreen());
        }
        
        public void UpdateFOV(float vel)
        {
            var t = Mathf.Clamp01(vel / 22f);

            var max = _fieldOfViewMaps[FieldOfViewWarpType.Type1].fieldOfViewWarpAttributes.maxFOV;
            
            var finalFieldOfView = Mathf.Lerp(60, max, t);

            _camera.fieldOfView = finalFieldOfView;
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
            var timer = 0f;
            var curve = effectsManagerData.fadeCurve;
            var fadeScreen = (FadeScreen)ScreenManager.Instance.GetScreen(ScreenType.FadeScreen);
            
            while (timer < effectsManagerData.fadeDuration)
            {
                timer += Time.deltaTime;
                
                var curveValue = curve.Evaluate(timer);
                var color = fadeScreen.fadeImage.color;
                color.a = curveValue;
                
                fadeScreen.fadeImage.color = color;
                
                yield return null;
            }
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
                var curveValue = curve.Evaluate(timer);
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
            
            /*if (pauseGameOnFinish)
            {
                GameManager.Instance.HandlePauseInput();
            }*/
        }
    }
}