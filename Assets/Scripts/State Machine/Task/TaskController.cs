using UnityEngine;

public class TaskController : MonoBehaviour
{
    private ITaskState _currentState;
    private ITaskState _previousState;

    public bool NeedsToBeDone { get; } = false;
    public UnavailableState UnavailableState { get; private set; } = new UnavailableState();
    public AvailableState AvailableState { get; private set; } = new AvailableState();
    public BeingDoneState BeingDoneState { get; private set; } = new BeingDoneState();
    private void OnEnable()
    {
        _currentState = UnavailableState;
        _previousState = _currentState;
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
}