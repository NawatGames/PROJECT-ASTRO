using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private PlayerInputController playerInputController;
    
    
    [SerializeField] private InputAction _pauseAction;
    private bool _isPaused;

    private void Start()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
        _pauseAction = playerInputController.pauseInputAction;
        _pauseAction.performed += Pause;
    }
    
    private void OnEnable()
    {
        if (_pauseAction != null)
        {
            _pauseAction.performed += Pause;
        }
        
    }

    private void OnDisable()
    {
        _pauseAction.performed -= Pause;
    }

    private void Pause(InputAction.CallbackContext ctx)
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
    }
}
