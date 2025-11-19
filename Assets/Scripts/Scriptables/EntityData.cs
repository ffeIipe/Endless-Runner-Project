using Enums;
using UnityEngine;

namespace Scriptables
{
    public abstract class EntityData : ScriptableObject
    {
        [Header("Attributes Settings")]
        public float health;
        
        [Header("Team Settings")]
        public Team team;
    }
}