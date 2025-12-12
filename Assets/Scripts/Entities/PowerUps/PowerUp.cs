using Interfaces;
using Managers;
using Pool;
using Scriptables.PowerUps;
using UnityEngine;

namespace Entities.PowerUps
{
    [RequireComponent(typeof(SphereCollider))]
    
    public abstract class PowerUp : MonoBehaviour, IPickable, IPoolable
    {
        [SerializeField] protected PowerUpData powerUpData;
        public Entity Owner { get; set; }
        
        private float _duration;
        private SphereCollider _collider;
        private CountdownTimer _timer;
        private MeshRenderer _meshRenderer;

        private bool _wasPickedUp;
        
         private Vector3 _initialPos;

        protected virtual void Awake()
        {
            _duration = powerUpData.duration;
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
        
        protected virtual void OnEnable()
        {
            _timer = new CountdownTimer(_duration);
            _timer.OnTimerStop += RemoveEffect;
            
            EventManager.GameEvents.OnLevelUpdated += OnLevelUpdated;
        }

        protected virtual void OnDisable()
        {
            _timer.OnTimerStop -= RemoveEffect;
            
            EventManager.GameEvents.OnLevelUpdated -= OnLevelUpdated;
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
            
            /*var displacement = powerUpData.amplitude * Mathf.Sin(Time.time * powerUpData.frequency);
            transform.position = _initialPos + new Vector3(0, displacement, 0);
            transform.Rotate(Vector3.up * (powerUpData.rotationSpeed * Time.deltaTime), Space.World);*/
        }

        private void OnLevelUpdated()
        {
            FactoryManager.Instance.ReturnObject(powerUpData.powerUpType, this);
        }

        public virtual void PickUp(Entity user)
        {
            if(_wasPickedUp) return;
            _wasPickedUp = true;
            
            _timer.Start();
            ApplyEffect(user);
            
            _meshRenderer.enabled = false;   
            
            if (_duration == 0) FactoryManager.Instance.ReturnObject(powerUpData.powerUpType, this);
            
            else StartCoroutine(FactoryManager.Instance.ReturnObjectWithLifeTime(powerUpData.powerUpType, this, _duration + 0.1f));
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
            if (!other.TryGetComponent(out Player.Player entity)) return;
            
            PickUp(entity);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _collider.enabled = true;
            _meshRenderer.enabled = true;
        }

        public void Deactivate()
        {
            _collider.enabled = false;
            gameObject.SetActive(false);
        }
    }
}