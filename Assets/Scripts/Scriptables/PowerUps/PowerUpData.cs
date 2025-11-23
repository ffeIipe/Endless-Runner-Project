using Enums;
using UnityEngine;

namespace Scriptables
{
    public abstract class PowerUpData : ScriptableObject
    {
        public float duration;
        public PoolableType powerUpType;
    }
}