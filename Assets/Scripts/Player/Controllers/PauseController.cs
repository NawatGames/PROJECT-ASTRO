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
    private bool _inputsFrozen;

    private void Start()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
        _pauseAction = playerInputController.pauseInputAction;
        _pauseAction.performed += Pause;
        _inputsFrozen = false;
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
        _inputsFrozen = !_inputsFrozen;
        Time.timeScale = _isPaused ? 0f : 1f;
    }

    public bool IsFrozen()
    {
        return _inputsFrozen;
    }
}
