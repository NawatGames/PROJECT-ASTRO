using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskScript : MonoBehaviour
{
    protected PlayerInputAsset inputAsset;
    protected bool isAstro; // Podera ser usada no futuro para vantagens em task de acordo com o personagem
    protected bool isAstroSpecialist;
    protected bool isTaskInProgress = false;
    private TaskController _taskController;
    [SerializeField] private TasksManager tasksManager;

    protected virtual void Awake()
    {
        _taskController = GetComponentInParent<TaskController>();
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
        isTaskInProgress = false;
        tasksManager.TaskDoneSuccessfully(_taskController);
    }

    protected virtual void TaskMistakeStay() // Player errou, mas continua no estado DoingTask
    {
        Debug.Log("Task Mistake (stay)");
        _taskController.Mistakes ++;
    }

    protected virtual void TaskMistakeLeave() // Player errou e sai do estado DoingTask
    {
        Debug.Log("Task Mistake (leave)");
        isTaskInProgress = false;
        _taskController.Mistakes ++;
        tasksManager.KickPlayer(_taskController);
    }

    public virtual void EndTask()
    {
        StopAllCoroutines();
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
        //Debug.Log("Iniciou Task: " + this);
        int specialistRng;
        if(isTaskInProgress == false)
        {
            specialistRng = Random.Range(1, 100);
            if(specialistRng >= 50)
            {
                Debug.Log(specialistRng);
                isAstroSpecialist = true;
                isTaskInProgress = true;
                Debug.Log("Astro is the specialist of this task");
            }

            else
            {
                Debug.Log(specialistRng);
                isAstroSpecialist = false;
                isTaskInProgress = true;
                Debug.Log("Astro is not the specialist of this task");
            }
        }
    }
}
