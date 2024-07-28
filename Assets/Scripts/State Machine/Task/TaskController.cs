using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = System.Object;

public class TaskController : MonoBehaviour
{
    public ITaskState currentState;
    private ITaskState _previousState;

    public TaskScript taskScript;
    public string taskName; // Para ser exibido na UI

    public bool needsToBeDone = false;
    public bool wasStarted;
    public bool wasInterrupted;
    public UnavailableState UnavailableState { get; private set; } = new UnavailableState();
    public AvailableState AvailableState { get; private set; } = new AvailableState();
    public BeingDoneState BeingDoneState { get; private set; } = new BeingDoneState();

    [SerializeField] private int _maxMistakes = 7; // Usar um unico valor pra todas tasks? -> Usar scriptableObject
    /*debug*/[SerializeField] private int _mistakes = 0;
    public int Mistakes { get => _mistakes; set => _mistakes = value > _maxMistakes ? _maxMistakes : value; }

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

    public void ResetMistakes() // Chamada no fim da invasão do monstro, no AlienBehavior
    {
        Mistakes = 0;
    }
}