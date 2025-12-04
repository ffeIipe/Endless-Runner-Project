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
            
            _spawnPoints = new List<SpawnPoint>();
            
            var spawnPointsInChildren = gameObject.GetComponentsInChildren<SpawnPoint>();
            foreach (var spawnPoint in spawnPointsInChildren)
            {
                _spawnPoints.Add(spawnPoint);
            }
        }

        private void OnEnable()
        {
            if (_trigger != null) _trigger.OnTriggered += SpawnEnemies;

            else Debug.LogError("No trigger found");
        }

        private void OnDisable()
        {
            if (_trigger != null) _trigger.OnTriggered -= SpawnEnemies;
            
            else Debug.LogError("No trigger found");
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