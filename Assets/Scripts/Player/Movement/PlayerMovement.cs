using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private Rigidbody2D rb;
        [Header("Settings")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float driftFactor = 0.05f;
        [SerializeField] private float acceleration = 10f;
        
        private PlayerInputAsset _inputAsset;
        private Vector2 _direction;
        private Vector2 _currentVelocity;
        private void Awake()
        {
            _inputAsset = new PlayerInputAsset();
            input.actions = _inputAsset.asset;
        }

        private void OnEnable()
        {
            _inputAsset.Default.Movement.performed += MovementOnPerformed;
            _inputAsset.Default.Movement.canceled += MovementOnPerformed;
        }
        
        private void OnDisable()
        {
            _inputAsset.Default.Movement.performed -= MovementOnPerformed;
            _inputAsset.Default.Movement.canceled -= MovementOnPerformed;
        }

        private void MovementOnPerformed(InputAction.CallbackContext obj)
        {
            _direction = obj.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _currentVelocity = Vector2.Lerp(
                _currentVelocity,
                _direction * moveSpeed,
                acceleration * Time.fixedDeltaTime
            );
            rb.velocity = rb.velocity * driftFactor + _currentVelocity * (1 - driftFactor);
        }
    }
}