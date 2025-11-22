using Entities.Enemies;
using Enums;
using Managers;
using UnityEngine;

namespace Obstacles
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private PoolableType enemyToSpawn;

        public void Spawn()
        {
            FactoryManager.Instance.Spawn<Enemy>(
                enemyToSpawn,
                transform.position,
                transform.rotation,
                null
            );
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }
}