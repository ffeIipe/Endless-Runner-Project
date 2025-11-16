using Entities.Enemies;
using UnityEngine;

namespace Obstacles
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Enemy enemyToSpawn;

        public void Spawn()
        {
            EnemyFactory.Instance.SpawnEnemy(transform);
        }
    }
}