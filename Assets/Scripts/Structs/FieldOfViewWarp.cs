using System;
using Enums;
using UnityEngine;

namespace Structs
{
    [Serializable]
    public struct FieldOfViewWarp
    {
        public float minFOV;
        public float maxFOV;

        public FieldOfViewWarp(float minFOV,float maxFOV) 
        { 
            this.minFOV =  minFOV;
            this.maxFOV = maxFOV;
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