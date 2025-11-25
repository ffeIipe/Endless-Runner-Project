using Enums;
using UnityEngine;

namespace Scriptables.Entities
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "BulletData", order = 0)]
    public class BulletData : ScriptableObject
    {
        [Header("Pool Settings")]
        public PoolableType poolableType;
        public float timeBeforeDeactivate;
        
        [Header("Bullet Settings")]
        public float bulletForce;
        public float damage;
    }
}