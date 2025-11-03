using UnityEngine;

namespace Entities.MVC
{
    public class Controller
    {
        private Model _model;
        
        public Controller(Model model)
        {
            _model = model;
        }
        
        public void ExecuteController()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _model.IsGrounded())
                _model.Jump();
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
                _model.Slide();
            
            if (Input.GetKeyUp(KeyCode.LeftShift))
                _model.ResetSlide();
            
            _model.Look();
            /*_model.IsGrounded();*/
        }

        public void ExecuteFixedController()
        {
            _model.Move();
        }
    }
}