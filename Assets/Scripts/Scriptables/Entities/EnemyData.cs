using Enums;
using UnityEngine;

namespace Scriptables.Entities
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptables/EnemyData")]
    public class EnemyData : EntityData
    {
        [Header("Factory Pool ID")]
        public PoolableType poolableType;
        
        [Header("Patrol/Idle Settings")]
        public float maxPatrolDistance;

        [Header("Attack Settings")]
        public float attackDistance;
        public float attackCooldown;
        public float interpSpeed;
        public int spread;
        
        [Header("Vision Settings")]
        public LayerMask targetLayer;
        public LayerMask obstacleLayer;
        public float scanInterval;
        
        [Header("Attack Settings")]
        public float jumpDuration;
        public AnimationCurve jumpCurve;
        public float jumpHeight;
        
        [Header("Effects Settings")]
        public float dissolveEffectDuration;
        public float timeUntilDeactivation;
    }
}