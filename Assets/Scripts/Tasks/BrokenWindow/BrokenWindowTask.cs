using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class BrokenWindowTask : TaskScript
{
    [SerializeField] private int sequenceSize = 5;
    [SerializeField] private BrokenWindowTimerBar timerBarScript;
    private List<DirectionEnum> _sequence;
    private Dictionary<DirectionEnum, char> _arrowsMapping;
    
    
    private enum DirectionEnum
    {
        Up,
        Down,
        Left,
        Right
    }

    protected override void Awake()
    {
        base.Awake();
        _sequence = new List<DirectionEnum>();
        timerBarScript.timeOutEvent.AddListener(TaskMistakeLeave);
    }

    protected override void RunTask()
    {
        base.RunTask();
        timerBarScript.ResetTimerBarSize();
        CreateNewSequence();
        StartCoroutine(timerBarScript.StartTimer());
        _arrowsMapping = new Dictionary<DirectionEnum, char>()
        {
            { DirectionEnum.Up, '↑' },
            { DirectionEnum.Down, '↓' },
            { DirectionEnum.Left, '←' },
            { DirectionEnum.Right, '→' }
        };
        Debug.Log(GetSequenceString());
    }

    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        base.OnUpPerformed(value);
        ReadInput(DirectionEnum.Up);
    }

    protected override void OnRightPerformed(InputAction.CallbackContext value)
    {
        base.OnRightPerformed(value);
        ReadInput(DirectionEnum.Right);
    }

    protected override void OnLeftPerformed(InputAction.CallbackContext value)
    {
        base.OnLeftPerformed(value);
        ReadInput(DirectionEnum.Left);
    }

    protected override void OnDownPerformed(InputAction.CallbackContext value)
    {
        base.OnDownPerformed(value);
        ReadInput(DirectionEnum.Down);
    }

    private void ReadInput(DirectionEnum input)
    {
        if (input == _sequence[0])
        {
            _sequence.RemoveAt(0);
            Debug.Log(GetSequenceString());
            if (_sequence.Count == 0)
            {
                TaskSuccessful();
            }
        }
        else
        {
            TaskMistakeStay();
            CreateNewSequence();
            Debug.Log(GetSequenceString());
        }
    }

    private void CreateNewSequence()
    {
        _sequence.Clear();
        for (int i = 0; i < sequenceSize; i++)
        {
            _sequence.Add((DirectionEnum)Random.Range(0,4));
        }
    }

    private string GetSequenceString()
    {
        string strSequence = "";
        foreach (DirectionEnum d in _sequence)
        {
            strSequence += _arrowsMapping[d];
        }
        return strSequence;
    }
}
