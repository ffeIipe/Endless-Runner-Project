using Entities;
using Entities.MVC;
using UnityEngine;

namespace Player
{
    public class Player : Entity
    {
        private Model _model;
        private Controller _controller;
        private View _view;
        private CharacterController _characterController;
        private bool _enabled;

        public CharacterController GetCharacterController() => _characterController;
        
        protected override void Awake()
        {
            base.Awake();

            _enabled = true;
            
            _characterController = GetComponent<CharacterController>();
            _characterController.stepOffset = 0;
            _characterController.skinWidth = 0;
            _characterController.center = new Vector3 (0f, GetComponentInChildren<CapsuleCollider>().height * 0.5f, 0f);
            
            _model = new Model(this, entityData);
            _controller = new Controller(_model, StartCoroutine);
            _view = new View(_model);
        }

        protected override void Dead()
        {
            base.Dead();
            
            _characterController.enabled = false;
        }

        protected void Update()
        {
            if (_enabled)
            {
                _controller.Execute();
            }
        }

        protected void FixedUpdate()
        {
            if (_enabled)
            {
                _controller.FixedExecute();
            }
        }

        public override void PauseEntity(bool pause)
        {
            base.PauseEntity(pause);

            if (pause)
            {
                _enabled = false;
            }
            else
            {
                _enabled = true;
            }
        }
    }
}
