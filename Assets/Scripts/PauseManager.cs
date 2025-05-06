using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private float repeatedPauseDelay = .5f;
    [SerializeField] private GameObject pauseMenuGameObject;
    [SerializeField] private GameEvent pauseToggled;
    
    private bool _isPaused;
    private float _lastPauseTime;
    
    public void Pause() // called by inputtedPause GameEvent
    {
        if (Time.unscaledTime >= _lastPauseTime + repeatedPauseDelay)
        {
            //Debug.Log((_isPaused?"un":"") + "paused");
            if (!_isPaused)
            {
                _lastPauseTime = Time.unscaledTime;
                _isPaused = true;
                pauseMenuGameObject.SetActive(true);
                Time.timeScale = 0f;
                pauseToggled.Raise(true);
            
            }
            else
            {
                _isPaused = false;
                pauseMenuGameObject.SetActive(false);
                Time.timeScale = 1f;
                pauseToggled.Raise(false);
            }
        }
    }
}
