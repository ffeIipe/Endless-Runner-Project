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

        protected override void Awake()
        {
            base.Awake();

            _characterController = GetComponent<CharacterController>();
            _characterController.stepOffset = 0;
            _characterController.skinWidth = 0;
            _characterController.center = new Vector3 (0f, GetComponentInChildren<CapsuleCollider>().height * 0.5f, 0f);
            
            _model = new Model(this, entityData);
            _controller = new Controller(_model, StartCoroutine);
            _view = new View(_model);
           
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected void Update()
        {
            _controller.Execute();
        }

        protected void FixedUpdate()
        {
            _controller.FixedExecute();
        }

        public CharacterController GetCharacterController() => _characterController;
    }
}
