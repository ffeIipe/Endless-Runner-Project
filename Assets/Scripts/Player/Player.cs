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
        private View _view;
        private CharacterController _characterController;

        public CharacterController GetCharacterController() => _characterController;
        
        protected override void Awake()
        {
            base.Awake();

            GetAttributesComponent().OnDead += () =>
            {
                EventManager.PlayerEvents.OnPlayerDead.Invoke();
                Cursor.lockState = CursorLockMode.None;
            };
            
            _characterController = GetComponent<CharacterController>();
            _characterController.stepOffset = 0;
            _characterController.skinWidth = 0;
            _characterController.center = new Vector3 (0f, GetComponentInChildren<CapsuleCollider>().height * 0.5f, 0f);
            
            _model = new Model(this, PlayerData);
            _controller = new Controller(_model, StartCoroutine);
            _view = new View(_model);
        }

        protected override void Start()
        {
            base.Start();

            GameManager.Instance.player = this;
            
            EventManager.UIEvents.OnSensitivityChanged += _model.ChangeSensitivity;
            EventManager.UIEvents.OnSoundVolumeChanged += _view.UpdateVolumeSound;
        }
        
        protected override void Dead()
        {
            base.Dead();
            
            _controller.Enabled = false;
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

            if (pause)
            {
                _controller.Enabled = false;
            }
            else
            {
                _controller.Enabled = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}
