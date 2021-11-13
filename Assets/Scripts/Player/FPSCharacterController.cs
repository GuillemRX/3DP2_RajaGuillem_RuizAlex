using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Player
{
    public class FPSCharacterController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 8f;
        [SerializeField] [Range(0, 1)] private float smoothing;
        [SerializeField] private float jumpHeight = 2;
        [SerializeField] private float jumpApexTime = 1;

        private Vector2 _horizontalVelocity, _currentVelocity, _direction;
        private Vector3 _currentRotation;
        private bool _isJumping;
        private float _jumpSpeed, _gravity;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _horizontalVelocity = Vector2.zero;
            _gravity = 2 * jumpHeight / (jumpApexTime * jumpApexTime);
            _jumpSpeed = 2 * jumpHeight / jumpApexTime;
        }

        private void Update()
        {
            _horizontalVelocity = GetHorizontalVelocity();

            var t = transform;
            var x = _horizontalVelocity.x * t.right;
            var z = _horizontalVelocity.y * t.forward;
            var y = (_rigidbody.velocity.y - _gravity * Time.deltaTime) * Vector3.up;
            
            _rigidbody.velocity = x + y + z;

            transform.eulerAngles = transform.eulerAngles.y * Vector3.up;
        }

        private void OnEnable()
        {
            EventManager.StartListening(Events.Instance.inputActions.onMove, OnMove);
            EventManager.StartListening(Events.Instance.inputActions.onJump, OnJump);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Events.Instance.inputActions.onMove, OnMove);
            EventManager.StopListening(Events.Instance.inputActions.onJump, OnJump);
        }

        private Vector2 GetHorizontalVelocity()
        {
            var targetSpeed = _direction * movementSpeed;
            return Vector2.SmoothDamp(_horizontalVelocity, targetSpeed, ref _currentVelocity, smoothing);
        }

        private void OnMove(Dictionary<string, object> message)
        {
            _direction = (Vector2) message["value"];
        }

        private void OnJump(Dictionary<string, object> message)
        {
            _isJumping = (bool) message["value"];
            
            if (!_isJumping) return;
            
            var velocity = _rigidbody.velocity;
            velocity = new Vector3(velocity.x, _jumpSpeed, velocity.z);
            _rigidbody.velocity = velocity;
        }
    }
}