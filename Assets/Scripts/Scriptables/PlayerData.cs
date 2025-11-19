using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptables/PlayerData")]
    public class PlayerData : EntityData
    {
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

        [Header("Controller Settings")] 
        public float mouseSensitivity;

    }
}
