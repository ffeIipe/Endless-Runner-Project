using System;
using Enums;
using UnityEngine;

namespace Structs
{
    [Serializable]
    public struct FieldOfViewWarp
    {
        public float maxFOV;
        public AnimationCurve curve;

        public FieldOfViewWarp(float maxFOV, AnimationCurve curve) 
        { 
            this.maxFOV = maxFOV;
            this.curve = curve;
        }
        
        public void UpdateEffect(float t)
        {
            throw new NotImplementedException();
        }
    }
    
    [Serializable]
    public class FieldOfViewMap
    {
        public FieldOfViewWarpType fieldOfViewWarpType;
        public FieldOfViewWarp fieldOfViewWarpAttributes;
    }
}