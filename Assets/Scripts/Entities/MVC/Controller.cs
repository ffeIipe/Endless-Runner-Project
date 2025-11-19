using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Entities.MVC
{
    public class Controller
    {
        public bool Enabled;
        private readonly Model _model;
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;
        
        public Controller(Model model, Func<IEnumerator, Coroutine> startCoroutine)
        {
            _model = model;
            _startCoroutine = startCoroutine;

            Enabled = true;
        }
        
        public void Execute()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                GameManager.Instance.TogglePause();
            
            if (!Enabled) return;
            
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
            if (!Enabled) return;
            
            _model.ApplyGravity();
            _model.Move();
        }
    }
}