using System;
using Enums;
using UnityEngine;

namespace Structs
{
    [Serializable]
    public struct TimeWarp
    {
        public float min;
        public float max;
        public AnimationCurve curve;

        public TimeWarp(float min, float max, AnimationCurve curve) 
        { 
            this.min = min; 
            this.max = max;
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