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
        public float minAttackCooldown;
        public float maxAttackCooldown;
        public float interpSpeed;
        public int spread;
        
        [Header("Vision Settings")]
        public LayerMask targetLayer;
        public LayerMask obstacleLayer;
        public float scanInterval;
        
        [Header("Effects Settings")]
        public float dissolveEffectDuration;
        public float timeUntilDeactivation;
    }
}