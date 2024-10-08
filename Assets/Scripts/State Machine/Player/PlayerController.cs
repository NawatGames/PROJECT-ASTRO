using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float driftFactor = 0.05f;
    [SerializeField] private float acceleration = 10f;
    
    public PlayerInputAsset Input;
    public bool isAstro = true; // true se for astronauta; false se for Alien
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Vector2 MoveVector { get; private set; } = Vector2.zero;
    public bool IsOnTaskArea { get; private set; }
    public bool IsOnLobbyArea { get; private set; }
    public bool isDoingDecontamination;
    public GameEvent startedDecontaminationEvent;
    public GameEvent stoppedDecontaminationEvent;
    public bool IsOnButtonArea { get; private set; }
    public bool GameIsOver { get; private set; }
    public TaskController NearTaskController { get; private set; }
    public DoorButtonController NearDoorButtonController { get; private set; }
    public InputAction InteractAction { get; private set; }
    public float MoveSpeed => moveSpeed;
    public float DriftFactor => driftFactor;
    public float Acceleration => acceleration;

    private IPlayerState _currentState;
    private IPlayerState _previousState;
    public FreeMovingStateOld FreeMovingStateOld { get; private set; } = new FreeMovingStateOld();
    public DoingTasksStateOld DoingTasksStateOld { get; private set; } = new DoingTasksStateOld();
    public GameOverStateOld GameOverStateOld { get; private set; } = new GameOverStateOld();
    public DoingDecontaminationStateOld DoingDecontaminationStateOld { get; private set; } = new DoingDecontaminationStateOld();
    public WalkingTowardsTaskStateOld WalkingTowardsTaskStateOld { get; private set; } = new WalkingTowardsTaskStateOld();

    private void Awake() // No awake, a variável isAstro ainda não está setada (mas no Start sim)
    {
        Input = new PlayerInputAsset();
        PlayerInput pInput = GetComponent<PlayerInput>();
        pInput.actions = Input.asset;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        InteractAction = Input.Default.Interaction;
    }

    private void OnEnable()
    {
        Input.Enable();
        Input.Default.Movement.performed += OnMovementPerformed;
        Input.Default.Movement.canceled += OnMovementCancelled;
        _currentState = FreeMovingStateOld;
        _previousState = _currentState;
    }

    private void OnDisable()
    {
        Input.Disable();
        Input.Default.Movement.performed -= OnMovementPerformed;
        Input.Default.Movement.canceled -= OnMovementCancelled;
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
    }
    
    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        MoveVector = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isLobby = other.CompareTag("Lobby");

        if (isTask || isQuarantineButton || isLobby)
        {
            if (isLobby)
            {
                IsOnLobbyArea = true;
                return;
            }
            // Botão acima do player
            if (isTask)
            {
                IsOnTaskArea = true;
                NearTaskController = other.GetComponentInChildren<TaskController>();
                return;
            }
            NearDoorButtonController = other.GetComponentInParent<DoorButtonController>();
            IsOnButtonArea = true;
    
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isLobby = other.CompareTag("Lobby");

        if (isTask || isQuarantineButton || isLobby)
        {
            // Botão acima do player
            if(isLobby)
            {
                IsOnLobbyArea = false;
                return;
            }
            if (isTask)
            {
                IsOnTaskArea = false;
                NearTaskController = null;
                return;
            }
            NearDoorButtonController = other.GetComponentInParent<DoorButtonController>();
            IsOnButtonArea = false;
        }
    }

    public void OnCompletedDecontamination()
    {
        isDoingDecontamination = false;
    }

    public void SetGameOverState() // Chamada por evento
    {
        GameIsOver = true;
    }
}
