using System;
using Interfaces;
using Managers;
using Pool;
using Scriptables;
using UnityEngine;

namespace Entities.PowerUps
{
    [RequireComponent(typeof(SphereCollider))]
    
    public abstract class PowerUp : MonoBehaviour, IPickable, IPoolable
    {
        [SerializeField] protected PowerUpData powerUpData;
        public Entity Owner { get; set; }
        
        protected Action OnPicked = delegate { };
        
        private float _duration;
        private SphereCollider _collider;
        private CountdownTimer _timer;

        protected virtual void Awake()
        {
            _duration = powerUpData.duration;
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
        }
        
        protected virtual void OnEnable()
        {
            _timer = new CountdownTimer(_duration);
        }
        
        protected virtual void Start()
        {
            OnPicked += () => _timer.Start();
            _timer.OnTimerStop += RemoveEffect;
        }

        public virtual void PickUp(Entity user)
        {
            OnPicked.Invoke();
            
            ApplyEffect(user);
            
            FactoryManager.Instance.ReturnObject(powerUpData.powerUpType, this);
        }

        public void Drop()
        {
        }

        protected virtual void ApplyEffect(Entity user)
        {
            Owner = user;
        }
        
        protected virtual void RemoveEffect() { }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Entity entity) || entity != GameManager.Instance.player) return;
            
            PickUp(entity);
            
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _collider.enabled = true;
        }

        public void Deactivate()
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }
    }
}