using System;
using Scriptables;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    
    public abstract class Entity : MonoBehaviour
    {
        public EntityData entityData;
        private Rigidbody _rigidbody;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public Rigidbody GetRigidbody() => _rigidbody;
    }
}