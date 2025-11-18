using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptables/EnemyData")]
    public class EnemyData : EntityData
    {
        [Header("Patrol/Idle Settings")]
        public float maxPatrolDistance;

        [Header("Attack Settings")]
        public float attackDistance;
        public float attackCooldown;
        
        [Header("Vision Settings")]
        public LayerMask targetLayer;
        public LayerMask obstacleLayer;
        public int maxTargets;
        
        [Header("Effects Settings")]
        public float dissolveEffectDuration;
    }
}