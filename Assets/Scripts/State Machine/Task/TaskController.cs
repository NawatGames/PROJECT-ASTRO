using UnityEngine;

public class TaskController : MonoBehaviour
{
    public ITaskState currentState;
    private ITaskState _previousState;

    public TaskScript taskScript;
    public string taskName; // Para ser exibido na UI

    public bool needsToBeDone = false;
    public bool wasStarted;
    public bool wasInterrupted;
    
    // TODO: Por enquanto esses dois GameEvent são usados no mesmo momento e poderiam ser apenas um
    [SerializeField] private GameEvent taskBecameAvailable;
    [SerializeField] private GameEvent taskBecameInaccessible;

    public UnavailableState UnavailableState { get; private set; } = new UnavailableState();
    public AvailableState AvailableState { get; private set; } // setado no OnEnable
    public BeingDoneState BeingDoneState { get; private set; } = new BeingDoneState();

    [SerializeField] private int _maxMistakes = 7; // Usar um unico valor pra todas tasks? -> Usar scriptableObject
    /*debug*/[SerializeField] private int _mistakes = 0;
    public int Mistakes { get => _mistakes; set => _mistakes = value > _maxMistakes ? _maxMistakes : value; }

    public Transform playerPositioning;
    [SerializeField] private StatusLight statusLight;
    public StatusLight StatusLight { get => statusLight; private set => statusLight = value; }
    public GameObject brokenTaskMask;
    
    
    private void OnEnable()
    {
        AvailableState = new AvailableState(taskBecameAvailable, taskBecameInaccessible);

        currentState = UnavailableState;
        _previousState = currentState;
        if (taskScript is null)
        {
            Debug.LogWarning("Há um objeto task Sem script! Criar componente e associar no TaskController");
        }
    }

    private void Update()
    {
        currentState = currentState.Do(this);
        if (_previousState != currentState)
        {
            //Debug.Log(currentState);
            _previousState.Exit(this);
            currentState.Enter(this);
        }
        _previousState = currentState;
    }

    private void FixedUpdate()
    {
        currentState.FixedDo(this);
    }

    public void ResetMistakes() // Chamada no fim da invasão do monstro, no AlienBehavior
    {
        Mistakes = 0;
    }
}