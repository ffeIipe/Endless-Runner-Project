using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Scriptables/EntityData")]
    public class EntityData : ScriptableObject
    {
        public float health;
        public float damage;
        public float speed;
        public float jumpForce;
    }
}