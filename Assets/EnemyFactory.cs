using Entities.Enemies;
using Pool;
using Scriptables;
using UnityEngine;

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
            , o =>
            {
                o.GetComponent<IPoolable>().Activate();
            }
            , o =>
            {
                o.GetComponent<IPoolable>().Deactivate();
            }
            , poolData.poolSize
        );
    }

    public Enemy SpawnEnemy(Transform origin)
    {
        var newEnemy = _enemyPool.GetObject();
        newEnemy.GetNavMeshAgent().transform.SetPositionAndRotation(origin.position, origin.rotation);
            
        newEnemy.GetComponent<IPoolable>()?.Activate();
            
        return newEnemy;
    }
}
