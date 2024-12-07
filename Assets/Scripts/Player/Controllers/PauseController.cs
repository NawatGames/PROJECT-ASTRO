using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private GameObject pauseMenuGameObject;
    
    
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

    public void Pause(InputAction.CallbackContext ctx)
    {
        _isPaused = !_isPaused;
        _inputsFrozen = !_inputsFrozen;
        // Debug.Log("pause");
        if (_isPaused)
        {
            pauseMenuGameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuGameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void PauseClick()
    {
        _isPaused = !_isPaused;
        // Debug.Log("pause");
        _inputsFrozen = !_inputsFrozen;
        if (_isPaused)
        {
            pauseMenuGameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuGameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public bool IsFrozen()
    {
        return _inputsFrozen;
    }
}
