using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "PoolData", menuName = "PoolData", order = 0)]
    public class PoolData : ScriptableObject
    {
        [Header("Pool Settings")]
        public MonoBehaviour prefabToSpawn;
        public int poolSize;
    }
}