using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float driftFactor = 0.95f;
    [SerializeField] private float acceleration = 2f;
    
    public Rigidbody2D Rigidbody2D { get; private set; }
    public KeyCode MoveUpKey => moveUpKey;
    public KeyCode MoveDownKey => moveDownKey;
    public KeyCode MoveLeftKey => moveLeftKey;
    public KeyCode MoveRightKey => moveRightKey;
    
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
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _currentState = FreeMovingState;
    }

    private void Update()
    {
        _currentState = _currentState.Do(this);
    }
}
