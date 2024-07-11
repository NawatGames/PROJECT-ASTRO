using System;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public ITaskState currentState;
    private ITaskState _previousState;

    public TaskScript taskScript;

    public bool needsToBeDone = false;
    public bool wasStarted;
    public bool wasInterrupted;
    public UnavailableState UnavailableState { get; private set; } = new UnavailableState();
    public AvailableState AvailableState { get; private set; } = new AvailableState();
    public BeingDoneState BeingDoneState { get; private set; } = new BeingDoneState();
    private void OnEnable()
    {
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
}