using Enums;
using UnityEngine;

namespace Scriptables.PowerUps
{
    public abstract class PowerUpData : ScriptableObject
    {
        [Header("Idle Animation")]
        public float amplitude = 0.001f;
        public float frequency = 1f;
        public float rotationSpeed = 50f;
        
        public float duration;
        public PoolableType powerUpType;
    }
}