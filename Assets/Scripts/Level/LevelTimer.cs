using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private int countdownTillVictory = 600;
    [SerializeField] private GameEvent onVictory;
    private TextMeshProUGUI _levelTimerTMP;
    private int _min;
    private int _sec;

    private void Awake()
    {
        _levelTimerTMP = GetComponent<TextMeshProUGUI>();
        _min = countdownTillVictory / 60;
        _sec = countdownTillVictory - 60 * _min;
        StartCoroutine(DecreaseTimer());
    }

    private IEnumerator DecreaseTimer()
    {
        _levelTimerTMP.text = $"Level Timer: {_min,2}:{_sec:00}";
        while (_sec > 0 || _min > 0)
        {
            yield return new WaitForSeconds(1);
            _sec--;
            if (_sec < 0)
            {
                _min--;
                _sec = 59;
            }
            _levelTimerTMP.text = $"Level Timer: {_min,2}:{_sec:00}";
        }
        Debug.Log("Victory");
        onVictory.Raise();
    }
}
