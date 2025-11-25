using Entities;
using Enums;
using UnityEngine;

namespace Scriptables.Abilities
{
    public abstract class AbilityData : ScriptableObject
    {
        public float cooldown;
        public PoolableType poolableType;
    }
}
