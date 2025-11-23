using Enums;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "PoolData", menuName = "PoolData", order = 0)]
    public class PoolData : ScriptableObject
    {
        [Tooltip("ID who will call the Factory later to retrieve an object.")]
        public PoolableType poolableType;

        [Tooltip("IPoolable prefab.")]
        public GameObject prefab;

        public int prewarmSize = 10;
    }
}