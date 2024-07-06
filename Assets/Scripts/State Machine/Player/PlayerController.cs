using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float driftFactor = 0.05f;
    [SerializeField] private float acceleration = 10f;

    private AstronautInput _input = null;
    
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Vector2 MoveVector { get; private set; } = Vector2.zero;
    public KeyCode InteractKey => interactKey;
    public float MoveSpeed => moveSpeed;
    public float DriftFactor => driftFactor;
    public float Acceleration => acceleration;

    private IPlayerState _currentState;
    private IPlayerState _previousState;
    public FreeMovingState FreeMovingState { get; private set; } = new FreeMovingState();
    public DoingTasksState DoingTasksState { get; private set; } = new DoingTasksState();
    public GameOverState GameOverState { get; private set; } = new GameOverState();

    private void Awake()
    {
        _input = new AstronautInput();
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Default.Movement.performed += OnMovementPerformed;
        _input.Default.Movement.canceled += OnMovementCancelled;
        _currentState = FreeMovingState;
        _previousState = _currentState;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Default.Movement.performed -= OnMovementPerformed;
        _input.Default.Movement.canceled -= OnMovementCancelled;
    }

    private void Update()
    {
        _currentState = _currentState.Do(this);
        if (_previousState != _currentState)
        {
            _previousState.Exit(this);
            _currentState.Enter(this);
        }
        _previousState = _currentState;
    }

    private void FixedUpdate()
    {
        _currentState.FixedDo(this);
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        MoveVector = value.ReadValue<Vector2>();
        Debug.Log(MoveVector);
    }
    
    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        MoveVector = Vector2.zero;
    }
}
