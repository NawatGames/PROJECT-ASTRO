using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private PlayerInputController playerInputController;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private PlayerAnimationController playerAnimationController;
        [Header("Settings")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float driftFactor = 0.05f;
        [SerializeField] private float acceleration = 10f;
        
        private InputAction _movementAction;
        private Vector2 _inputDirection;
        private Vector2 _currentVelocity;

        private void Start()
        {
            _movementAction = playerInputController.movementInputAction;
            _movementAction.performed += OnMovement;
            _movementAction.canceled += OnMovement;
        }

        private void OnEnable()
        {
            if (_movementAction != null)
            {
                _movementAction.performed += OnMovement;
                _movementAction.canceled += OnMovement;
            }
        }

        private void OnDisable()
        {
            rb.velocity = Vector2.zero;
            _inputDirection = Vector2.zero;
            _currentVelocity = Vector2.zero;
            _movementAction.performed -= OnMovement;
            _movementAction.canceled -= OnMovement;
        }

        private void OnMovement(InputAction.CallbackContext obj)
        {
            _inputDirection = obj.ReadValue<Vector2>();
            playerAnimationController.SetMovementAnimParameters(_inputDirection);
        }

        private void SetVelocity(Vector2 direction)
        {
            _currentVelocity = Vector2.Lerp(
                _currentVelocity,
                direction * moveSpeed,
                acceleration * Time.fixedDeltaTime
            );
            rb.velocity = rb.velocity * driftFactor + _currentVelocity * (1 - driftFactor);
        }

        private void FixedUpdate()
        {
            SetVelocity(_inputDirection);
        }
        
        // RODA COM ESTE SCRIPT DESATIVADO: TODO: Ao inves de usar Coroutine, passar esse codigo para o FixedUpdate e usar evento ao inves da Action switchToTargetState
        public IEnumerator GoToTarget(Vector2 targetPosition, Action switchToTargetState)
        {
            Vector2 playerPos = transform.position;
            
            _currentVelocity = Vector2.zero;
            // Trocar linha acima pela de baixo para evitar que o player pare (Também é necessario nao zerar o rb.velocity)
            //_currentVelocity = Mathf.Clamp(Vector2.Dot((targetPosition - playerPos).normalized, rb.velocity),0,Mathf.Infinity) * rb.velocity.normalized;

            Vector2 direction;
            while (true)
            {
                direction = targetPosition - playerPos;
                playerAnimationController.SetMovementAnimParameters(direction);
                SetVelocity(direction.normalized);
            
                if ((rb.velocity).magnitude * Time.fixedDeltaTime >= direction.magnitude)
                {
                    rb.velocity = Vector2.zero;
                    rb.MovePosition(targetPosition);
                    break;
                }
            
                yield return new WaitForFixedUpdate();
                playerPos = transform.position;
            }
            _currentVelocity = Vector2.zero;
            playerAnimationController.ForceIdleWithDirection(direction.normalized);
            switchToTargetState();
        }
    }