using UnityEngine;

namespace Scriptables.Abilities
{
    [CreateAssetMenu(fileName = "Parry", menuName = "Scriptables/Abilities/Parry")]
    public class ParryData : AbilityData
    {
        public float duration;
        public float radiusRange;
        public float parryForce;
    }
}
