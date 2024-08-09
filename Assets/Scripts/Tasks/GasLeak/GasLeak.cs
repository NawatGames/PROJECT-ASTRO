using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GasLeak : TaskScript
{
    enum KeyDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    enum Rotation
    {
        Clockwise,
        Counterclockwise
    }
    
    [Header("Task Config")]
    [SerializeField] private float taskTime = 15f;
    [SerializeField] private int maxTurnCount = 10;
    [SerializeField] private int mistakePenalty = 5;
    [SerializeField] private Rotation rotationDirection = Rotation.Clockwise;
    
    [Header("Visual")]
    [SerializeField] private Transform valveSprite;
    
    private int _taskProgress = 0;
    private int _currentKeyIndex = 0;
    private float _timeLeft;
    private KeyDirection _lastKey;
    private bool _resetSequence = true;
    
    private KeyDirection[] _keyOrder = {
        KeyDirection.Left,
        KeyDirection.Up,
        KeyDirection.Right,
        KeyDirection.Down
    };

    protected override void Awake()
    {
        base.Awake();
        // Reverse rotation if enabled
        if (rotationDirection == Rotation.Counterclockwise) Array.Reverse(_keyOrder);
    }
    
    protected override void RunTask()
    {
        base.RunTask();
        _taskProgress = 0;
        _currentKeyIndex = 0;
        _timeLeft = taskTime;
        _resetSequence = true;
        StartCoroutine(DecayTimeBar());
    }
    
    private void TurnValve(KeyDirection key)
    {
        // Set first key in sequence
        if (_resetSequence)
        {
            _lastKey = key;
            _resetSequence = false;
            return;
        }
        // Gets next key in sequence
        int correctKeyIndex = Array.IndexOf(_keyOrder, _lastKey) + 1;
        
        if (key != _keyOrder[correctKeyIndex % _keyOrder.Length])
        {
            // Apply progress penalty
            _currentKeyIndex = _currentKeyIndex > mistakePenalty ? _currentKeyIndex - mistakePenalty : 0;
            _resetSequence = true;
            TaskMistakeStay();
            return;
        }
        
        // Increase progress
        _lastKey = key;
        _currentKeyIndex++;
        _taskProgress = _currentKeyIndex / _keyOrder.Length;
        
        // Rotate valve sprite
        Vector3 valveRotation = valveSprite.rotation.eulerAngles;
        valveRotation.z += rotationDirection == Rotation.Clockwise ? -15 : 15f;
        valveSprite.transform.rotation = Quaternion.Euler(valveRotation);
        
        // Ends task if turn count is fulfilled
        if (_taskProgress >= maxTurnCount) TaskSuccessful();
    }
   
    // Task time countdown
    IEnumerator DecayTimeBar()
    {
        while (_timeLeft > 0)
        {
            yield return new WaitForFixedUpdate();
            _timeLeft -= Time.fixedDeltaTime;
        }
        TaskMistakeLeave();
    }
    
    // Input listeners
    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        TurnValve(KeyDirection.Up);
    }
    
    protected override void OnDownPerformed(InputAction.CallbackContext value)
    {
        TurnValve(KeyDirection.Down);
    }
    
    protected override void OnLeftPerformed(InputAction.CallbackContext value)
    {
        TurnValve(KeyDirection.Left);
    }
    
    protected override void OnRightPerformed(InputAction.CallbackContext value)
    {
        TurnValve(KeyDirection.Right);
    }
}