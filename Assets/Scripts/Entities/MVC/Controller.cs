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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _model.Jump();
            }
        }

        public void ExecuteFixedController()
        {
            _model.Move();
        }
    }
}