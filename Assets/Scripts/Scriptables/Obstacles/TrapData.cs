using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "TrapData", menuName = "Scriptables/TrapData")]
    public class TrapData : ScriptableObject
    {
        public float rotationAngle;
        public float rotationDuration;
        public float damage;
    }
}