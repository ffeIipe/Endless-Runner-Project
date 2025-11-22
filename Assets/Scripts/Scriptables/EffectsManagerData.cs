using System.Collections.Generic;
using Structs;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "EffectsManagerData", menuName = "Scriptables/EffectsManagerData")]
    public class EffectsManagerData : ScriptableObject
    {
        public List<HitStopMap> hitStopMap;
        public List<TimeWarpMap> timeWarpMap;
        public List<FieldOfViewMap> fieldOfViewMap;
    }
}