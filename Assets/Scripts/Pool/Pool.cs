using System;
using System.Collections.Generic;

namespace Pool
{
    public class Pool<T>
    {
        private readonly Func<T> _factoryMethod;

        private readonly Action<T> _turnOnCallback;
        private readonly Action<T> _turnOffCallback;

        private readonly List<T> _currentStock;

        public Pool(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialAmount)
        {
            _currentStock = new List<T>();
            _factoryMethod = factoryMethod;
            _turnOnCallback = turnOnCallback;
            _turnOffCallback = turnOffCallback;

            for (var i = 0; i < initialAmount; i++)
            {
                var obj = _factoryMethod();
                _turnOffCallback(obj);
                _currentStock.Add(obj);
            }
        }

        public T GetObject()
        {
            T result;

            if (_currentStock.Count == 0)
            {
                result = _factoryMethod();
            }
            else
            {
                result = _currentStock[0];
                _currentStock.RemoveAt(0);
            }

            _turnOnCallback(result);
    
            return result;
        }

        public void ReturnObjectToPool(T obj)
        {
            _turnOffCallback(obj);
            _currentStock.Add(obj);
        }
    }
}