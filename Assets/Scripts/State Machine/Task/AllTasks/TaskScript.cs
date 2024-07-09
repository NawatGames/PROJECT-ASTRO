using UnityEngine;
using UnityEngine.InputSystem;

public class TaskScript : MonoBehaviour
{
    private InputActionAsset _inputAsset;
    private InputActionMap _taskInputMap;
    protected bool isAstro; // Podera ser usada no futuro para vantagens em task de acordo com o personagem
    
    public void SetupAndRun(InputActionAsset pInputAsset, bool pIsAstro)
    {
        _inputAsset = pInputAsset;
        isAstro = pIsAstro;
        _taskInputMap = _inputAsset.FindActionMap("Task");
        _taskInputMap.Enable();
        _taskInputMap.FindAction("Up").performed += OnUpPerformed;
        _taskInputMap.FindAction("Down").performed += OnDownPerformed;
        _taskInputMap.FindAction("Left").performed += OnLeftPerformed;
        _taskInputMap.FindAction("Right").performed += OnRightPerformed;
        RunTask();
    }

    public void RemoveInput()
    {
        _taskInputMap.Disable();
        _taskInputMap.FindAction("Up").performed -= OnUpPerformed;
        _taskInputMap.FindAction("Down").performed -= OnDownPerformed;
        _taskInputMap.FindAction("Left").performed -= OnLeftPerformed;
        _taskInputMap.FindAction("Right").performed -= OnRightPerformed;
    }

    protected virtual void OnUpPerformed(InputAction.CallbackContext value) {}
    protected virtual void OnDownPerformed(InputAction.CallbackContext value) {}
    protected virtual void OnLeftPerformed(InputAction.CallbackContext value) {}
    protected virtual void OnRightPerformed(InputAction.CallbackContext value) {}
    
    
    protected virtual void RunTask()
    {
        
    }
}
