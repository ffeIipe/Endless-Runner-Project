using System.Collections;
using Factories;
using Scriptables;
using UnityEngine;

namespace Entities.MVC
{
    public class Model
    {
        private readonly Entity _owner;
        private readonly CharacterController _characterController;
        private readonly EntityData _entityData;

        private Vector3 _lastMovementVector;
        
        private Vector3 _currentStrafeVelocity;
        private Vector3 _currentForwardVelocity;
        
        private readonly Vector3 _localScale;
        private bool _isSliding;

        private readonly Camera _camera;
        private float _xRotation;
        private float _mouseSensitivity;
        private const float Gravity = -9.81f;

        public Model(Entity owner, EntityData entityData)
        {
            _owner = owner;
            _entityData = entityData;
            _mouseSensitivity = entityData.mouseSensitivity;
            _localScale = _owner.transform.localScale;
            _camera = Camera.main;
            _characterController = _owner.GetComponent<CharacterController>();
        }

        public void ApplyGravity()
        {
            var gravity = Vector3.down * (Gravity * Time.fixedDeltaTime);
            _characterController.Move(-gravity);
        }

        public void Move()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");

            var forwardVec = _owner.transform.forward;
            var rightVec = _owner.transform.right;

            var targetForwardVelocity = forwardVec * (verticalInput * _entityData.maxSpeed);
            var targetStrafeVelocity = rightVec * (horizontalInput * _entityData.maxSpeed);

            _currentForwardVelocity = Vector3.MoveTowards(
                _currentForwardVelocity,
                targetForwardVelocity,
                _entityData.acceleration * Time.fixedDeltaTime
            );

            _currentStrafeVelocity = Vector3.MoveTowards(
                _currentStrafeVelocity,
                targetStrafeVelocity,
                _entityData.acceleration * Time.fixedDeltaTime
            );

            var finalVelocity = _currentForwardVelocity + _currentStrafeVelocity;

            if (finalVelocity.magnitude > _entityData.maxSpeed && !_isSliding)
            {
                finalVelocity = finalVelocity.normalized * _entityData.maxSpeed;
            }

            var finalForce = new Vector3(
                finalVelocity.x,
                0,
                finalVelocity.z
            );

            _characterController.Move(finalForce * Time.fixedDeltaTime);

            _lastMovementVector = finalVelocity.normalized;
        }

        public IEnumerator Jump()
        {
            var timer = 0f;
            var lastCurveValue = 0f;

            while (timer < _entityData.jumpDuration)
            {
                timer += Time.deltaTime;

                var percent = timer / _entityData.jumpDuration;
                var curveValue = _entityData.jumpCurve.Evaluate(percent) * _entityData.jumpHeight;
                var moveDeltaY = (curveValue - lastCurveValue);
                var moveVector = new Vector3(0, moveDeltaY, 0);

                _characterController.Move(moveVector);

                lastCurveValue = curveValue;

                yield return null;
            }
        }

        public bool IsGrounded()
        {
            var origin = _owner.transform.position + new Vector3(0, .25f, 0);
            var distance = _entityData.jumpMinDistanceAttempt;
            
            Debug.DrawLine(_owner.transform.position,
                origin + Vector3.down * distance, Color.blue, 1f);
            
            return Physics.Raycast(
                origin,
                Vector3.down,
                distance,
                LayerMask.GetMask("Ground")
            );
        }

        public IEnumerator Slide()
        {
            if (_isSliding) yield break;
            
            _isSliding = true;
            
            var newScale = new Vector3(
                _owner.transform.localScale.x,
                _owner.transform.localScale.y / 2f,
                _owner.transform.localScale.z
            );
            
            _owner.transform.localScale = newScale;
            
            var timer = 0f;
            var lastCurveValue = 0f;
            var previousLastMovementVector = _lastMovementVector;
                
            while (timer < _entityData.slideDuration)
            {
                timer += Time.deltaTime;

                var percent = timer / _entityData.slideDuration;
                var curveValue = _entityData.slideCurve.Evaluate(percent) * _entityData.slideDistance;
                var moveDeltaX = curveValue - lastCurveValue;

                _characterController.Move(previousLastMovementVector * moveDeltaX);

                lastCurveValue = curveValue;

                yield return null;
            }
            
            ResetSlide();
        }

        private void ResetSlide()
        {
            _owner.transform.localScale = _localScale;
            _isSliding = false;
        }

        public void Look()
        {
            var mouseX = Input.GetAxisRaw("Mouse X") * _mouseSensitivity * Time.deltaTime;
            var mouseY = Input.GetAxisRaw("Mouse Y") * _mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _owner.transform.Rotate(Vector3.up * mouseX);
        }

        public void ThrowAxe()
        {
            var bullet = BulletFactory.Instance.SpawnBullet(_owner.handPoint, _owner);
            var finalForce = _currentForwardVelocity +  _currentStrafeVelocity;
            bullet.Fire(_owner.handPoint.forward, _owner.transform.rotation,finalForce * 0.5f);
        }
    }
}