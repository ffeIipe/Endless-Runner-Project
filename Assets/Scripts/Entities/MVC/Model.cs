using Scriptables;
using UnityEngine;

namespace Entities.MVC
{
    public class Model
    {
        private readonly Entity _owner;
        private readonly CharacterController _characterController;
        private readonly EntityData _entityData;

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
            _owner.transform.position += gravity;
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

            var finalHorizontalVelocity = _currentForwardVelocity + _currentStrafeVelocity;

            if (finalHorizontalVelocity.magnitude > _entityData.maxSpeed && !_isSliding)
            {
                finalHorizontalVelocity = finalHorizontalVelocity.normalized * _entityData.maxSpeed;
            }
            
            var finalForce = new Vector3(
                finalHorizontalVelocity.x,
                0,
                finalHorizontalVelocity.z
            );
            
            _characterController.Move(finalForce * Time.fixedDeltaTime);
        }
        
        public void Jump()
        {
            _characterController.Move(Vector3.up * _entityData.jumpForce);
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

        public void Slide()
        {
            _isSliding = true;
            var direction = _camera.transform.forward;
            
            var force = direction * _entityData.slideForce;
            _owner.GetRigidbody().AddForce(force, ForceMode.VelocityChange);
            
            var newScale = new Vector3(
                _owner.transform.localScale.x,
                _owner.transform.localScale.y / 2f,
                _owner.transform.localScale.z
            );

            _owner.transform.localScale = newScale;
        }

        public void ResetSlide()
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
            
        }
    }
}