using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "LogData", menuName = "LogData", order = 0)]
    public class LogData : ScriptableObject
    {
        public float damage;
        public float speed;
        public float rotationRate;
        public float duration;
    }
}