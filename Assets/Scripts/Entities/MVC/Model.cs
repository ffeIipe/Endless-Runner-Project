using System;
using System.Collections;
using Enums;
using Managers;
using Scriptables.Entities;
using UnityEngine;

namespace Entities.MVC
{
    public class Model
    {
        public Action<float> OnVelocityChanged = delegate { };
        
        private readonly Entity _owner;
        private readonly CharacterController _characterController;
        private readonly PlayerData _playerData;

        private Vector3 _lastMovementVector;
        
        private Vector3 _currentVelocity;
        
        private readonly Vector3 _localScale;
        private bool _isSliding;
        private bool _stopSlideRequest; 
        
        private bool _stopJumpRequest; 

        private readonly Camera _camera;
        private float _xRotation;
        private float _mouseSensitivity;
        private const float Gravity = -9.81f;
        
        private readonly CountdownTimer _attackCooldown;
        private bool _canAttack;
        
        private readonly CountdownTimer _abilityCooldown;
        private bool _canAbilty;

        public Model(Entity owner, PlayerData playerData)
        {
            _owner = owner;
            _playerData = playerData;
            _mouseSensitivity = playerData.mouseSensitivity;
            _localScale = _owner.transform.localScale;
            _camera = Camera.main;
            _characterController = _owner.GetComponent<CharacterController>();

            _canAttack = true;
            _attackCooldown = new CountdownTimer(playerData.timeBetweenAttacks);
            _attackCooldown.OnTimerStop += () => _canAttack = true;

            _canAbilty = true;
            _abilityCooldown = new CountdownTimer(_playerData.abilityData.cooldown);
            _abilityCooldown.OnTimerStop += () => _canAbilty = true;
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

            var targetVelocity = forwardVec * (verticalInput * _playerData.maxSpeed) + rightVec * (horizontalInput * _playerData.maxSpeed);

            _currentVelocity = Vector3.MoveTowards(_currentVelocity, targetVelocity, Time.deltaTime * _playerData.acceleration);

            var finalForce = new Vector3(
                _currentVelocity.x,
                0,
                _currentVelocity.z
            );

            _characterController.Move(finalForce * Time.fixedDeltaTime);

            _lastMovementVector = _currentVelocity.normalized;
            
            OnVelocityChanged.Invoke(_currentVelocity.magnitude);
        }

        private void CancelJump()
        {
            _stopJumpRequest = true;
        }

        public IEnumerator Jump()
        {
            _stopJumpRequest = false;
            
            CancelSlide();
            
            var timer = 0f;
            var lastCurveValue = 0f;

            while (timer < _playerData.jumpDuration)
            {
                if (_stopJumpRequest)
                {
                    yield break;
                }

                if (!GameManager.IsPaused) 
                {
                    timer += Time.deltaTime;

                    var percent = timer / _playerData.jumpDuration;
                    var curveValue = _playerData.jumpCurve.Evaluate(percent) * _playerData.jumpHeight;
                    var moveDeltaY = (curveValue - lastCurveValue);
                    var moveVector = new Vector3(0, moveDeltaY, 0);

                    _characterController.Move(moveVector);

                    lastCurveValue = curveValue;
                }
                
                yield return null;
            }
        }

        public bool IsGrounded()
        {
            var castOriginOffset = _playerData.castOriginOffset; 
            var baseOrigin = _owner.transform.position + Vector3.up * castOriginOffset;
            
            var checkDistance = castOriginOffset + _playerData.jumpMinDistanceAttempt;

            Vector3[] offsets =
            {
                Vector3.zero,
                Vector3.forward * _playerData.groundCheckRadius,
                Vector3.back * _playerData.groundCheckRadius,
                Vector3.right * _playerData.groundCheckRadius,
                Vector3.left * _playerData.groundCheckRadius
            };

            foreach (var offset in offsets)
            {
                var worldOffset = _owner.transform.TransformDirection(offset);
                var rayOrigin = baseOrigin + worldOffset;

                if (Physics.Raycast(rayOrigin, Vector3.down, out _, checkDistance, _playerData.groundMask))
                {
                    return true;
                }
            }

            return false;
        }

        private void CancelSlide()
        {
            _stopSlideRequest = true;
        }

        public IEnumerator Slide()
        {
            if (_isSliding) yield break;
            
            _isSliding = true;
            _stopSlideRequest = false;

            CancelJump();
            SnapToGround(); 

            var targetScale = new Vector3(
                _owner.transform.localScale.x,
                _owner.transform.localScale.y / 2f,
                _owner.transform.localScale.z
            );
            
            var timer = 0f;
            var lastCurveValue = 0f;
            var previousLastMovementVector = _lastMovementVector;
            
            while (timer < _playerData.slideDuration)
            {
                if (!GameManager.IsPaused)
                {
                    timer += Time.deltaTime;

                    var percent = timer / _playerData.slideDuration;
                    var curveValue = _playerData.slideCurve.Evaluate(percent) * _playerData.slideDistance;
                    var moveDeltaX = curveValue - lastCurveValue;

                    _characterController.Move(previousLastMovementVector * moveDeltaX);

                    lastCurveValue = curveValue;
                    
                    _owner.transform.localScale = Vector3.Lerp(
                        _owner.transform.localScale, 
                        targetScale, 
                        Time.deltaTime * _playerData.scaleSmoothingSpeed
                    );
                }
                
                if (_stopSlideRequest)
                { 
                    break;
                }

                yield return null;
            }
            
            _isSliding = false;
            _stopSlideRequest = false;

            _owner.StartCoroutine(ResetSlide());
        }

        private IEnumerator ResetSlide()
        {
            var timer = 0f;
            var duration = _playerData.slideResetDuration;
            var startScale = _owner.transform.localScale;

            while (timer < duration)
            {
                if (!GameManager.IsPaused)
                {
                    timer += Time.deltaTime;
                    var progress = timer / duration;
                    
                    _owner.transform.localScale = Vector3.Lerp(startScale, _localScale, progress);
                }
                yield return null;
            }

            _owner.transform.localScale = _localScale;
        }
        
        private void SnapToGround()
        {
            _owner.StartCoroutine(DoSnapToGround());
        }

        private IEnumerator DoSnapToGround()
        {
            if (Physics.Raycast(_owner.transform.position, Vector3.down, out var hit, _playerData.groundSnapDistance, _playerData.groundMask))
            {
                var timer = 0f;
                var duration = _playerData.snapDuration; 
                
                const float startDistance = 0f;
                var targetDistance = hit.distance;
                var lastDistanceMoved = 0f;

                while (timer < duration && _isSliding && !_stopSlideRequest)
                {
                    if (!GameManager.IsPaused)
                    {
                        timer += Time.deltaTime;
                        var t = timer / duration;
                        var currentTotalDrop = Mathf.Lerp(startDistance, targetDistance, t);
                        var deltaToMove = currentTotalDrop - lastDistanceMoved;
                        
                        _characterController.Move(Vector3.down * deltaToMove);
                        
                        lastDistanceMoved = currentTotalDrop;
                    }
                    
                    yield return null;
                }
                
                if (_isSliding && !_stopSlideRequest && lastDistanceMoved < targetDistance)
                {
                     _characterController.Move(Vector3.down * (targetDistance - lastDistanceMoved));
                }
            }
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
            if (!_canAttack) return;

            _canAttack = false;

            var bullet = FactoryManager.Instance.Spawn<Bullet>(
                PoolableType.Bullet,
                _owner.handPoint.position,
                _owner.handPoint.rotation,
                _owner
            );

            var aimDirection = _owner.handPoint.forward;
            var projectedSpeed = Vector3.Dot(_currentVelocity, aimDirection);
            var velocityToTransfer = aimDirection * projectedSpeed;

            bullet.Fire(aimDirection, velocityToTransfer);

            _attackCooldown.Start();
        }

        public void ExecuteCooldowns()
        {
            _attackCooldown.Tick(Time.deltaTime);
            _abilityCooldown.Tick(Time.deltaTime);
        }
        
        public void ChangeSensitivity(float newSens)
        {
            _mouseSensitivity = Mathf.Lerp(10f, 1000f, newSens);
        }

        public void UseAbility()
        {
            if (!_canAbilty) return;
                
            _canAbilty = false;
            
            FactoryManager.Instance.SpawnObject(
                PoolableType.ParryAbility,
                _owner.transform.position,
                Quaternion.identity,
                _owner
            );
            
            _abilityCooldown.Start();
        }
    }
}