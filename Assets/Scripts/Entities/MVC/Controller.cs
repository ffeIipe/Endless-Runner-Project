using System;
using System.Collections;
using UnityEngine;

namespace Entities.MVC
{
    public class Controller
    {
        private readonly Model _model;
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;
        
        public Controller(Model model, Func<IEnumerator, Coroutine> startCoroutine)
        {
            _model = model;
            _startCoroutine = startCoroutine;
        }
        
        public void Execute()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _model.IsGrounded())
                _startCoroutine(_model.Jump());
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
                _startCoroutine(_model.Slide());
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
                _model.ThrowAxe();
            
            _model.Look();
        }

        public void FixedExecute()
        {
            _model.ApplyGravity();
            _model.Move();
        }
    }
}