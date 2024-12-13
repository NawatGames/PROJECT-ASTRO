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
    
    
    private InputAction _pauseAction;
    private bool _isPaused;
    private bool _inputsFrozen;

    private InputActionMap _currentActionMap;

    private void Start()
    {
        _pauseAction = playerInputController.pauseInputAction;
        _inputsFrozen = false;
    }
    
    private void OnEnable()
    {
        if (_pauseAction != null)
        {
            _pauseAction.performed += Pause;
            playerInputController.inputAsset.Menu.Pause.performed += Pause;
        }
        
    }

    private void OnDisable()
    {
        _pauseAction.performed -= Pause;
        playerInputController.inputAsset.Menu.Pause.performed -= Pause;
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        _isPaused = !_isPaused;
        _inputsFrozen = !_inputsFrozen;
        // Debug.Log("pause");
        if (_isPaused)
        {
            _currentActionMap = playerInputController.input.currentActionMap;
            Debug.Log(_currentActionMap);
            playerInputController.inputAsset.Task.Disable();
            playerInputController.inputAsset.Default.Disable();
            playerInputController.inputAsset.Menu.Enable();
            pauseMenuGameObject.SetActive(true);
            Time.timeScale = 0f;
            
        }
        else
        {
            playerInputController.inputAsset.Menu.Disable();
            playerInputController.input.currentActionMap = _currentActionMap;
            playerInputController.input.ActivateInput();
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
