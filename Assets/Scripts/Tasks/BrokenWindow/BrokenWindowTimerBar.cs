using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BrokenWindowTimerBar : MonoBehaviour
{
    [SerializeField] private float initialTimerSeconds = 10;
    private Vector3 _localScale;
    private float _currentTimerSeconds;
    private float _initialSizeY;
    public UnityEvent timeOutEvent;

    private void Awake()
    {
        _localScale = transform.localScale;
        _initialSizeY = _localScale.y;
    }

    public void ResetTimerBarSize()
    {
        transform.localScale = new Vector3(_localScale.x, _initialSizeY, _localScale.z);
    }

    public IEnumerator StartTimer()
    {
        _currentTimerSeconds = initialTimerSeconds;
        while (_currentTimerSeconds > 0)
        {
            _currentTimerSeconds -= Time.deltaTime;
            transform.localScale = new Vector3(_localScale.x, _initialSizeY * _currentTimerSeconds / initialTimerSeconds, _localScale.z);
            yield return null;
        }
        timeOutEvent.Invoke();
    }
}
