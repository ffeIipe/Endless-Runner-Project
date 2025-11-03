using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Scriptables/EntityData")]
    public class EntityData : ScriptableObject
    {
        public float health;
        
        public float damage;
        
        public float acceleration;
        public float maxSpeed;
        public float jumpForce;
        
        public float mouseSensitivity;
        public float jumpMinDistanceAttempt;
        public float slideForce;
    }
}