using System;
using Interfaces;
using Scriptables;
using UnityEngine;

namespace Entities.PowerUps
{
    [RequireComponent(typeof(SphereCollider))]
    
    public abstract class PowerUp : MonoBehaviour, IPickable
    {
        [SerializeField] protected PowerUpData powerUpData;
        
        private float _duration;
        protected Action OnPicked = delegate { };
        
        private Entity _user;
        private CountdownTimer _timer;

        protected virtual void Awake()
        {
            _duration = powerUpData.duration;
            _timer = new CountdownTimer(_duration);
            
            OnPicked += () => _timer.Start();
            _timer.OnTimerStop += RemoveEffect;
        }
        
        public void PickUp(Entity user)
        {
            OnPicked.Invoke();
            
            ApplyEffect(user);
        }

        public void Drop()
        {
        }

        protected virtual void ApplyEffect(Entity user)
        {
            _user = user;
            
        }
        
        protected virtual void RemoveEffect() { }

        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponentInParent<Entity>(); //as the enemy is an entity, they can pick this power ups... feature(?
            if (entity == null) return;
            
            PickUp(entity);
        }
    }
}