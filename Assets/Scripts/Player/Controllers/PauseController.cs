using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private GameEvent inputtedPause;
    
    private InputAction _pauseAction;

    private InputActionMap _currentActionMap;
    
    private void OnEnable()
    {
        _pauseAction = playerInputController.pauseInputAction;
        _pauseAction.performed += Pause;
        playerInputController.inputAsset.Task.Pause.performed += Pause;
        playerInputController.inputAsset.Menu.Pause.performed += Pause;
    }

    private void OnDisable()
    {
        _pauseAction.performed -= Pause;
        playerInputController.inputAsset.Task.Pause.performed -= Pause;
        playerInputController.inputAsset.Menu.Pause.performed -= Pause;
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        inputtedPause.Raise();
    }

    public void PauseToggledHandler(Component _, object data)
    {
        if ((bool)data) // Pausou
        {
            _currentActionMap = playerInputController.input.currentActionMap;
            //Debug.Log(_currentActionMap);
            playerInputController.inputAsset.Task.Disable();
            playerInputController.inputAsset.Default.Disable();
            playerInputController.inputAsset.Menu.Enable();
        }
        else // Despausou
        {
            playerInputController.inputAsset.Menu.Disable();
            playerInputController.input.currentActionMap = _currentActionMap;
            playerInputController.input.ActivateInput();
        }
    }
}
