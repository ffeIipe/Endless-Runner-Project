using System;
using Entities.Enemies;
using Factories;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    }
}