using System.Collections.Generic;
using Structs;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "EffectsManagerData", menuName = "Scriptables/EffectsManagerData")]
    public class EffectsManagerData : ScriptableObject
    {
        [Header("Hit Stop Settings")]
        public List<HitStopMap> hitStopMap;
        
        [Header("Time Warp Settings")]
        public List<TimeWarpMap> timeWarpMap;
        
        [Header("FOV Settings")]
        public List<FieldOfViewMap> fieldOfViewMap;
        
        [Header("Fade Effect Settings")]
        public float fadeDuration;
        public AnimationCurve fadeCurve;
        
        [Header("Blood Effect Material")]
        public Material bloodRenderMaterial;
        public float bloodLerpSpeed = .5f;
        
        [Header("Wind Effect Settings")]
        public Material windRenderMaterial;
        public float windLerpSpeed = 2f;
        public string vignetteIntensity = "_VignetteIntensity";
    }
}