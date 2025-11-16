using System.Collections;
using Entities;
using Pool;
using Scriptables;
using UnityEngine;

namespace Factories
{
    public class BulletFactory : MonoBehaviour
    {
        public static BulletFactory Instance { get; private set; }
        
        [SerializeField] private PoolData poolData;
        private Pool<Bullet> _bulletPool;
        
        private void Awake()
        {
            Instance = this;

            if (Instance != this)
            {
                Destroy(gameObject);
            }
            
            _bulletPool = new Pool<Bullet>(
                () => Instantiate((Bullet)poolData.prefabToSpawn)
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

        public Bullet SpawnBullet(Transform origin, Entity owner)
        {
            var newBullet = _bulletPool.GetObject();
            newBullet.transform.SetPositionAndRotation(origin.position, origin.rotation);
            
            newBullet.GetComponent<IPoolable>()?.Activate();
            newBullet.SetOwner(owner);
            
            StartCoroutine(ReleaseAfterTime(newBullet));
            
            return newBullet;
        }

        private IEnumerator ReleaseAfterTime(Bullet newBullet)
        {
            yield return new WaitForSeconds(5f);
            _bulletPool.ReturnObjectToPool(newBullet);
        }
    }
}