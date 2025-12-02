using System;
using Entities;
using Entities.MVC;
using Managers;
using Scriptables.Entities;
using UnityEngine;

namespace Player
{
    public class Player : Entity
    {
        private PlayerData PlayerData => (PlayerData)entityData;
        private Model _model;
        private Controller _controller;
        private ViewPlayer _viewPlayer;
        private CharacterController _characterController;
        private CountdownTimer _bufferDamage;
        public CharacterController GetCharacterController() => _characterController;
        
        protected override void Awake()
        {
            base.Awake();
            
            _characterController = GetComponent<CharacterController>();
            _characterController.stepOffset = 0.01f;
            _characterController.skinWidth = 0.01f;
            _characterController.center = new Vector3 (0f, GetComponentInChildren<CapsuleCollider>().height * 0.5f, 0f);
            
            _model = new Model(this, PlayerData);
            _controller = new Controller(_model, StartCoroutine);
            _bufferDamage = new CountdownTimer(PlayerData.bufferDamage);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            SubscribeToEvents();
        }
        
        protected override void OnDisable() 
        {
            base.OnDisable();
            
            UnsubscribeToEvents();
        }

        private void Start()
        {
            Debug.Log("Start");
        }

        private void OnBufferDamageStop()
        {
            CanTakeDamage = true;
        }

        private void OnLevelFinished()
        {
            _bufferDamage.OnTimerStop -= OnBufferDamageStop;
            CanTakeDamage = false;
        }

        protected override void Die()
        {
            base.Die();
            
            EventManager.PlayerEvents.OnPlayerDead.Invoke();
            Cursor.lockState = CursorLockMode.None;

            _controller.Enabled = false;
        }

        protected override ViewBase InitializeView()
        {
            _viewPlayer = new ViewPlayer(this, _model);
            return _viewPlayer;
        }

        protected void Update()
        {
            _controller.Execute();
            
            _bufferDamage.Tick(Time.deltaTime);
        }

        protected void FixedUpdate()
        {
            _controller.FixedExecute();
        }

        public override void TakeDamage(float damage, Entity damageCauser)
        {
            base.TakeDamage(damage, damageCauser);

            CanTakeDamage = false;
            _bufferDamage.Start();

            EventManager.PlayerEvents.OnPlayerDamaged.Invoke();
            EventManager.UIEvents.OnHealthPercentageChanged?.Invoke(GetAttributesComponent().GetHealthPercentage());
        }

        public override void PauseEntity(bool pause)
        {
            base.PauseEntity(pause);

            _controller.Enabled = !pause;
            _viewPlayer.PauseEffects(pause);
        }

        protected override void OnLevelRestarted()
        {
            base.OnLevelRestarted();
            
            GetRigidbody().isKinematic = true;
            
            GetCharacterController().enabled = false;
            transform.rotation = Quaternion.identity;
            GetCharacterController().enabled = true;

            GetAttributesComponent().Reset();
            EventManager.UIEvents.OnHealthPercentageChanged?.Invoke(GetAttributesComponent().GetHealthPercentage());
            
            _controller.Enabled = true;
            
            _bufferDamage =  new CountdownTimer(PlayerData.bufferDamage);
        }
        
        private void SubscribeToEvents()
        {
            GameManager.Instance.player = this;
            
            EventManager.UIEvents.OnSensitivityChanged += _model.ChangeSensitivity;
            EventManager.GameEvents.OnLevelFinished += OnLevelFinished;
            
            _bufferDamage.OnTimerStop += OnBufferDamageStop;
            
            _model.OnVelocityChanged += _viewPlayer.GetVelocity;
        }
        
        private void UnsubscribeToEvents()
        {
            if(GameManager.Instance && GameManager.Instance.player == this) 
                GameManager.Instance.player = null;

            EventManager.UIEvents.OnSensitivityChanged -= _model.ChangeSensitivity;
            EventManager.GameEvents.OnLevelFinished -= OnLevelFinished;
            
            _bufferDamage.OnTimerStop -= OnBufferDamageStop;
            
            _model.OnVelocityChanged -= _viewPlayer.GetVelocity;
        }
    }
}
