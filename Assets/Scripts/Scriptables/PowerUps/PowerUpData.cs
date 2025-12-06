using Enums;
using UnityEngine;

namespace Scriptables.PowerUps
{
    public abstract class PowerUpData : ScriptableObject
    {
        public float duration;
        public PoolableType powerUpType;
    }
}