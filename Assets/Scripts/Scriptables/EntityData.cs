using Enums;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Scriptables/EntityData")]
    public class EntityData : ScriptableObject
    {
        [Header("Attributes Settings")]
        public float health;
        public float damage;
        
        [Header("Movement Settings")]
        public float acceleration;
        public float maxSpeed;
        
        [Header("Jump Settings")]
        public float jumpHeight;
        public float jumpDuration;
        public AnimationCurve jumpCurve;
        public float jumpMinDistanceAttempt;
        
        [Header("Slide Settings")]
        public float slideDistance;
        public float slideDuration;
        public AnimationCurve slideCurve;

        [Header("Controller Settings")] 
        public float mouseSensitivity;
        
        [Header("Team Settings")]
        public Team team;
    }
}