using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "WallTrapData", menuName = "WallTrapData", order = 0)]
    public class WallTrapData : ScriptableObject
    {
        public float timeTriggerSpike;
        public AnimationCurve spikeCurve;
        public float maxSpikesDistance;
        public float damage;
    }
}