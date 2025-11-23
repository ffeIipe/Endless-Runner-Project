using System;
using Enums;
using UnityEngine;

namespace Structs
{
    [Serializable]
    public struct HitStop
    {
        public AnimationCurve curve;
        public float minScale;
        public float duration;

        public HitStop(AnimationCurve curve)
        {
            this.curve = curve;
            minScale = 0;
            duration = 0;
        }
    }
    
    [Serializable]
    public class HitStopMap
    {
        public HitStopType hitStopType;
        public HitStop hitStopAttributes;
    }
}