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

    private void Awake()
    {
        _levelTimerTMP = GetComponent<TextMeshProUGUI>();
        StartCoroutine(DecreaseTimer());
    }

    private IEnumerator DecreaseTimer()
    {
        for (int timer = countdownTillVictory; timer > 0; timer--)
        {
            _levelTimerTMP.text = "Level Timer: " + timer;
            yield return new WaitForSecondsRealtime(1);
        }
        Debug.Log("Victory");
        onVictory.Raise();
    }
}
