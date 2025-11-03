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

        protected override void Awake()
        {
            base.Awake();
            
            Cursor.lockState = CursorLockMode.Locked;
            
            _model = new Model(this, entityData);
            _controller = new Controller(_model);
            _view = new View(_model);
        }
        
        protected void Update()
        {
            _controller.ExecuteController();
        }

        protected void FixedUpdate()
        {
            _controller.ExecuteFixedController();
        }
    }
}
