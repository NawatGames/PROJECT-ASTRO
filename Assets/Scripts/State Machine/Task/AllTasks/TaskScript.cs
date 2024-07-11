using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskScript : MonoBehaviour
{
    protected PlayerInputAsset inputAsset;
    protected bool isAstro; // Podera ser usada no futuro para vantagens em task de acordo com o personagem
    private TaskController taskCtrl;

    protected virtual void Awake()
    {
        taskCtrl = GetComponentInParent<TaskController>();
    }

    public void SetupAndRun(PlayerInputAsset pInputAsset, bool pIsAstro)
    {
        inputAsset = pInputAsset;
        isAstro = pIsAstro;
        inputAsset.Task.Enable();
        inputAsset.Task.Up.performed += OnUpPerformed;
        inputAsset.Task.Down.performed += OnDownPerformed;
        inputAsset.Task.Left.performed += OnLeftPerformed;
        inputAsset.Task.Right.performed += OnRightPerformed;
        RunTask();
    }

    protected virtual void TaskSuccessful()
    {
        taskCtrl.needsToBeDone = false;
    }

    public virtual void EndTask()
    {
        inputAsset.Task.Disable();
        inputAsset.Task.Up.performed -= OnUpPerformed;
        inputAsset.Task.Down.performed -= OnDownPerformed;
        inputAsset.Task.Left.performed -= OnLeftPerformed;
        inputAsset.Task.Right.performed -= OnRightPerformed;
    }

    protected virtual void OnUpPerformed(InputAction.CallbackContext value) {}
    protected virtual void OnDownPerformed(InputAction.CallbackContext value) {}
    protected virtual void OnLeftPerformed(InputAction.CallbackContext value) {}
    protected virtual void OnRightPerformed(InputAction.CallbackContext value) {}


    protected virtual void RunTask()
    {
        Debug.Log("Iniciou Task: " + this);
    }
}
