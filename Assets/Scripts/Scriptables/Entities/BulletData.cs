using Enums;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "BulletData", order = 0)]
    public class BulletData : ScriptableObject
    {
        [Header("Factory Pool ID")]
        public PoolableType poolableType;
        
        [Header("Bullet Settings")]
        public float bulletForce;
        public float damage;
    }
}