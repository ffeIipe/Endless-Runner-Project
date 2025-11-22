using Entities;
using Entities.MVC;
using Managers;
using Scriptables;
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

        public CharacterController GetCharacterController() => _characterController;
        
        protected override void Awake()
        {
            base.Awake();
            
            _characterController = GetComponent<CharacterController>();
            _characterController.stepOffset = 0;
            _characterController.skinWidth = 0;
            _characterController.center = new Vector3 (0f, GetComponentInChildren<CapsuleCollider>().height * 0.5f, 0f);
            
            _model = new Model(this, PlayerData);
            _controller = new Controller(_model, StartCoroutine);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            GetAttributesComponent().OnReceiveDamage += health =>
            {
                EventManager.UIEvents.OnHealthChanged?.Invoke(health);
                EventManager.PlayerEvents.OnPlayerDamaged.Invoke();
            };
            
            GetAttributesComponent().OnDead += () =>
            {
                EventManager.PlayerEvents.OnPlayerDead.Invoke();
                Cursor.lockState = CursorLockMode.None;
                
                _controller.Enabled = false;
            };
            
            EventManager.UIEvents.OnSensitivityChanged += _model.ChangeSensitivity;
            EventManager.UIEvents.OnHealthChanged?.Invoke(PlayerData.health);

            _model.OnVelocityChanged += _viewPlayer.GetVelocity;
        }
        
        protected void Start()
        {
            GameManager.Instance.player = this;
        }


        protected override ViewBase CreateView()
        {
            _viewPlayer = new ViewPlayer(this, _model);
            return _viewPlayer;
        }

        protected void Update()
        {
            _controller.Execute();
        }

        protected void FixedUpdate()
        {
            _controller.FixedExecute();
        }

        public override void PauseEntity(bool pause)
        {
            base.PauseEntity(pause);

            _controller.Enabled = !pause;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}
