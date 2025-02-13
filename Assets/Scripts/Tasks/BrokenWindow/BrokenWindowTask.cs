using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class BrokenWindowTask : TaskScript
{
    [SerializeField] private int sequenceSize;
    [SerializeField] private int normalSequenceSize = 5;
    [SerializeField] private int sequenceSizeSpecialist = 3;
    [SerializeField] private BrokenWindowTimerBar timerBarScript;
    [SerializeField] private GameObject upArrow;
    [SerializeField] private GameObject downArrow;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private ArrowsFrameManager frameManager;
    private List<DirectionEnum> _sequence;
    private Dictionary<DirectionEnum, GameObject> _arrowsMapping;
    [SerializeField] private int totalSequence;
    private int currentSequence;
    
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
        _arrowsMapping = new Dictionary<DirectionEnum, GameObject>()
        {
            { DirectionEnum.Up, upArrow },
            { DirectionEnum.Down, downArrow },
            { DirectionEnum.Left, leftArrow },
            { DirectionEnum.Right, rightArrow }
        };
        _sequence = new List<DirectionEnum>();
        timerBarScript.timeOutEvent.AddListener(TaskMistakeLeave);
    }

    protected override void RunTask()
    {
        currentSequence = 0;
        base.RunTask();
        timerBarScript.ResetTimerBarSize();
        
        if (isAstro == isAstroSpecialist) sequenceSize = sequenceSizeSpecialist;
        else  sequenceSize = normalSequenceSize;
        
        
        
        CreateNewSequence();
        ShowNewSequence();
        StartCoroutine(timerBarScript.StartTimer());
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
            frameManager.MarkArrow();
            _sequence.RemoveAt(0);
            if (_sequence.Count == 0)
            {
                currentSequence++;
                if (currentSequence >= totalSequence)
                {
                   TaskSuccessful();
                   _sequence.Clear();
                   frameManager.ResetArrowFrame();
                }
                else
                {
                    
                    CreateNewSequence();
                    ShowNewSequence();
                    StartCoroutine(timerBarScript.StartTimer());
                    
                }
            }
        }
        else
        {
            TaskMistakeLeave();
            _sequence.Clear();
            frameManager.ResetArrowFrame();
        }
    }

    private void CreateNewSequence()
    {
        _sequence.Clear();
        frameManager.ResetArrowFrame();
        for (int i = 0; i < sequenceSize; i++)
        {
            _sequence.Add((DirectionEnum)Random.Range(0,4));
        }
    }

    private void ShowNewSequence()
    {
        int index = 0;
        foreach (DirectionEnum d in _sequence)
        {
            frameManager.AddArrow(_arrowsMapping[d], index, sequenceSize);
            index++;
        }
    }
}
