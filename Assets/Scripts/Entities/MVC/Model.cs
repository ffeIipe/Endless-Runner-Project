using Scriptables;
using UnityEngine;

namespace Entities.MVC
{
    public class Model
    {
        private readonly Entity _owner;
        private readonly EntityData _entityData;

        private Vector3 _lastMaxSpeed;
        
        private readonly Vector3 _localScale;
        private bool _isSliding;

        private readonly Camera _camera;
        private float _xRotation;
        private float _mouseSensitivity;

        public Model(Entity owner, EntityData entityData)
        {
            _owner = owner;
            _entityData = entityData;
            _mouseSensitivity = entityData.mouseSensitivity;
            _localScale = _owner.transform.localScale;
            _camera = Camera.main;
        }

        private Vector3 _currentStrafeVelocity;
        private Vector3 _currentForwardVelocity;

        public void Move()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");

            var camForward = _camera.transform.forward;
            var camRight = _camera.transform.right;
            /*camForward.y = 0f;
            camRight.y = 0f;*/

            var targetForwardVelocity = camForward * (verticalInput * _entityData.maxSpeed);
            var targetStrafeVelocity = camRight * (horizontalInput * _entityData.maxSpeed);

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

            _owner.GetRigidbody().velocity = new Vector3(
                finalHorizontalVelocity.x,
                _owner.GetRigidbody().velocity.y,
                finalHorizontalVelocity.z
            );
        }
        
        public void Jump()
        {
            _owner.GetRigidbody().AddForce(Vector3.up * _entityData.jumpForce, ForceMode.Impulse);
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

        private void Pick()
        {

        }

        private void Interact()
        {

        }
    }
}