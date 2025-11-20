using Entities.Enemies;
using Pool;
using Scriptables;
using UnityEngine;

namespace Factories
{
    public class EnemyFactory : MonoBehaviour
    {
        public static EnemyFactory Instance { get; private set; }
        
        [SerializeField] private PoolData poolData;
        private Pool<Enemy> _enemyPool;

        private void Awake()
        {
            Instance = this;

            if (Instance != this)
            {
                Destroy(gameObject);
            }

            _enemyPool = new Pool<Enemy>(
                () => Instantiate((Enemy)poolData.prefabToSpawn)
                , enemy => { } 
                , enemy => enemy.Deactivate()
                , poolData.poolSize
            );
        }

        public Enemy SpawnEnemy(Transform origin)
        {
            var newEnemy = _enemyPool.GetObject();
            
            newEnemy.transform.SetLocalPositionAndRotation(origin.position, origin.rotation);    
            newEnemy.Activate();
            
            return newEnemy;
        }
    }
}