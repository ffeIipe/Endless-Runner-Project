using System;
using Enums;
using UnityEngine;

namespace Structs
{
    [Serializable]
    public struct TimeWarp
    {
        public float duration;
        public float targetScale;
        public AnimationCurve curve;

        public TimeWarp(float targetScale, float duration, AnimationCurve curve) 
        { 
            this.duration = duration;
            this.targetScale = targetScale; 
            this.curve = curve;
        }
    }
    
    [Serializable]
    public class TimeWarpMap
    {
        public TimeWarpType timeWarpType;
        public TimeWarp timeWarpAttributes;
    }
}