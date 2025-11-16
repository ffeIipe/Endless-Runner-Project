using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class EnemySpawner : MonoBehaviour
    {
        private List<SpawnPoint> _spawnPoints;
        private Trigger _trigger;
        
        private void Awake()
        {
            _trigger = GetComponentInChildren<Trigger>();
            _trigger.OnTriggered += SpawnEnemies;
            
            _spawnPoints = new List<SpawnPoint>();
            
            var spawnPointsInChildren = GetComponentsInChildren<SpawnPoint>();
            foreach (var spawnPoint in spawnPointsInChildren)
            {
                _spawnPoints.Add(spawnPoint);
            }
        }

        private void SpawnEnemies()
        {
            foreach (var spawnPoint in _spawnPoints)
            {
                spawnPoint.Spawn();
            }
        }
    }
}