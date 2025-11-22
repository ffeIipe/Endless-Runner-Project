using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
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

            /*foreach (var timeWarpMap in effectsManagerData.timeWarpMap)
            {
                _timeWarpMaps.Add(timeWarpMap.timeWarpType, timeWarpMap);
            }*/

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

        public void UpdateFOV(float vel)
        {
            var t = Mathf.Clamp01(vel / 22f);

            var max = _fieldOfViewMaps[FieldOfViewWarpType.Type1].fieldOfViewWarpAttributes.maxFOV;
            
            var finalFieldOfView = Mathf.Lerp(60, max, t);

            _camera.fieldOfView = finalFieldOfView;
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
            //_currentHitStopCoroutine = null;
        }
    }
}