using UnityEngine;

namespace Pool
{
    public interface IPoolHandler
    {
        IPoolable GetInstance();
        void ReturnInstance(IPoolable instance);
    }

    public class PoolHandler<T> : IPoolHandler where T : Component, IPoolable
    {
        private readonly Pool<T> _pool;

        public PoolHandler(T prefab, int size)
        {
            _pool = new Pool<T>(
                factoryMethod: () => Object.Instantiate(prefab),
                turnOnCallback: item => item.Activate(),
                turnOffCallback: item => item.Deactivate(),
                initialAmount: size
            );
        }

        public IPoolable GetInstance()
        {
            return _pool.GetObject();
        }

        public void ReturnInstance(IPoolable instance)
        {
            if (instance is T typedInstance)
            {
                _pool.ReturnObjectToPool(typedInstance);
            }
        }
    }
}