using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "BulletData", order = 0)]
    public class BulletData : ScriptableObject
    {
        public float bulletForce;
        public float damage;
    }
}