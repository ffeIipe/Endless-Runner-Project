using System.Collections;
using Entities;
using Managers;
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
                , bullet =>
                {
                    bullet.Activate();
                }
                , bullet =>
                {
                    bullet.Deactivate();
                }
                , poolData.poolSize
            );
        }

        public Bullet SpawnBullet(Transform origin, Entity owner)
        {
            var newBullet = _bulletPool.GetObject();
            newBullet.transform.SetPositionAndRotation(origin.position, origin.rotation);
            
            newBullet.Activate();
            newBullet.SetOwner(owner);
            
            StartCoroutine(ReleaseAfterTime(newBullet, 5f));
            
            return newBullet;
        }

        private IEnumerator ReleaseAfterTime(Bullet newBullet, float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                if (!GameManager.IsPaused)
                {
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                yield return null;
            }
            
            _bulletPool.ReturnObjectToPool(newBullet);
        }
    }
}