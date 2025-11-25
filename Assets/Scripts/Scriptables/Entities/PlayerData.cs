using Scriptables.Abilities;
using UnityEngine;

namespace Scriptables.Entities
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptables/PlayerData")]
    public class PlayerData : EntityData
    {
        [Header("Attributes Settings")]
        public float bufferDamage;
        
        [Header("Movement Settings")]
        public float acceleration;
        public float maxSpeed;
        
        [Header("Jump Settings")]
        public float jumpHeight;
        public float jumpDuration;
        public AnimationCurve jumpCurve;
        public float castOriginOffset;
        public float jumpMinDistanceAttempt;
        public float groundCheckRadius = .35f;
        public float groundSnapDistance = 1.5f;
        public LayerMask groundMask;
        
        [Header("Slide Settings")]
        public float slideDistance;
        public float slideDuration;
        public AnimationCurve slideCurve;
        public float scaleSmoothingSpeed;
        public float slideResetDuration;
        public float snapDuration;

        [Header("Attack Settings")]
        public float timeBetweenAttacks;
        
        [Header("Controller Settings")] 
        public float mouseSensitivity;
        
        [Header("Ability Settings")]
        public AbilityData abilityData;

    }
}
