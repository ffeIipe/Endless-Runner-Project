using Enums;
using UnityEngine;

namespace Scriptables
{
    public abstract class EntityData : ScriptableObject
    {
        [Header("Attributes Settings")]
        public float health;
        public float shield;
        
        [Header("Team Settings")]
        public TeamType teamType;
    }
}