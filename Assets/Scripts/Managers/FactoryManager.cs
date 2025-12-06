using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Enums;
using Pool;
using Scriptables;
using UnityEngine;

namespace Managers
{
    public class FactoryManager : MonoBehaviour
    {
        public static FactoryManager Instance { get; private set; }

        [Header("Pools")]
        [SerializeField] private List<PoolData> poolsToRegister;

        private readonly Dictionary<PoolableType, IPoolHandler> _pools = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            InitializePools();
        }

        private void OnEnable()
        { 
            //EventManager.GameEvents.OnLevelUpdated += TurnOffPools;
        }

        private void TurnOffPools()
        {
            /*foreach (var pool in _pools)
            {
                pool.Value.ReturnInstance();
            }*/
        }

        private void InitializePools()
        {
            foreach (var config in poolsToRegister)
            {
                RegisterPool(config);
            }
        }

        private void RegisterPool(PoolData config)
        {
            if (_pools.ContainsKey(config.poolableType)) return;

            var poolableComponent = config.prefab.GetComponent<IPoolable>();
            if (poolableComponent == null)
            {
                return;
            }

            var specificType = poolableComponent.GetType();
            var handlerType = typeof(PoolHandler<>).MakeGenericType(specificType);
            
            var poolHandler = (IPoolHandler)Activator.CreateInstance(handlerType, poolableComponent, config.prewarmSize);

            _pools.Add(config.poolableType, poolHandler);
        }

        public T Spawn<T>(PoolableType poolableType, Vector3 position, Quaternion rotation, Entity owner) where T : Component, IPoolable
        {
            if (!_pools.TryGetValue(poolableType, out var handler))
            {
                return null;
            }

            var item = handler.GetInstance();
            
            var typedItem = item as T;

            if (typedItem)
            {
                typedItem.transform.SetPositionAndRotation(position, rotation);
                typedItem.SetOwner(owner);
            }

            return typedItem;
        }
        
        public Component SpawnObject(PoolableType poolableType, Vector3 position, Quaternion rotation, Entity owner = null)
        {
            if (!_pools.TryGetValue(poolableType, out var handler))
            {
                return null;
            }

            var item = handler.GetInstance();

            if (item is Component componentItem)
            {
                componentItem.transform.SetPositionAndRotation(position, rotation);
                item.SetOwner(owner);
                return componentItem;
            }

            handler.ReturnInstance(item);
            return null;
        }

        public void ReturnObject(PoolableType poolableType, IPoolable obj)
        {
            if (_pools.TryGetValue(poolableType, out var handler))
            {
                handler.ReturnInstance(obj);
            }
        }
        
        public IEnumerator ReturnObjectWithLifeTime(PoolableType poolableType, IPoolable obj, float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                if (!GameManager.IsPaused)
                {
                    elapsedTime += Time.deltaTime;
                }
                
                yield return null;
            }

            ReturnObject(poolableType, obj);
        }
    }
}