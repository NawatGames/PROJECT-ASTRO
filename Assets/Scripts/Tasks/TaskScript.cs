using System;
using System.Collections;
using Audio_System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskScript : MonoBehaviour
{
    protected PlayerInputController inputController;
    protected bool isAstro; // Podera ser usada no futuro para vantagens em task de acordo com o personagem
    protected bool isAstroSpecialist;
    private bool isTaskInProgress = false;
    private TaskController _taskController;
    protected String taskName;

    [Header("AUDIO SAMPLES")]
    [SerializeField] private GameObject taskEnteredAudio;
    [SerializeField] private GameObject taskSuccessAudio;
    [SerializeField] private GameObject taskMistakeStayAudio;

    [Header("TASK CONFIG")]
    [SerializeField] private TasksManager tasksManager;
    

    protected virtual void Awake()
    {
        _taskController = GetComponentInParent<TaskController>();
    }

    public void SetupAndRun(PlayerInputController pInputController, bool pIsAstro)
    {
        inputController = pInputController;
        isAstro = pIsAstro;
        inputController.input.SwitchCurrentActionMap("Task");
        inputController.inputAsset.Task.Up.performed += OnUpPerformed;
        inputController.inputAsset.Task.Down.performed += OnDownPerformed;
        inputController.inputAsset.Task.Left.performed += OnLeftPerformed;
        inputController.inputAsset.Task.Right.performed += OnRightPerformed;
        RunTask();
    }

    protected virtual void TaskSuccessful()
    {
        isTaskInProgress = false;
        tasksManager.TaskDoneSuccessfully(_taskController);
        taskSuccessAudio.GetComponent<AudioPlayer>().PlayAudio();
    }

    protected virtual void TaskMistakeStay() // Player errou, mas continua no estado DoingTask
    {
        Debug.Log("Task Mistake (stay)");
        _taskController.Mistakes++;
        taskMistakeStayAudio.GetComponent<AudioPlayer>().PlayAudio();

    }

    protected virtual void TaskMistakeLeave() // Player errou e sai do estado DoingTask
    {
        Debug.Log("Task Mistake (leave)");
        isTaskInProgress = false;
        _taskController.Mistakes++;
        tasksManager.KickPlayer(_taskController);
    }

    public virtual void EndTask()
    {
        StopAllCoroutines();
        inputController.input.SwitchCurrentActionMap("Default");
        inputController.inputAsset.Task.Up.performed -= OnUpPerformed;
        inputController.inputAsset.Task.Down.performed -= OnDownPerformed;
        inputController.inputAsset.Task.Left.performed -= OnLeftPerformed;
        inputController.inputAsset.Task.Right.performed -= OnRightPerformed;
        isTaskInProgress = false;
    }

    protected virtual void OnUpPerformed(InputAction.CallbackContext value) { }
    protected virtual void OnDownPerformed(InputAction.CallbackContext value) { }
    protected virtual void OnLeftPerformed(InputAction.CallbackContext value) { }
    protected virtual void OnRightPerformed(InputAction.CallbackContext value) { }


    protected virtual void RunTask()
    {
        // FindObjectOfType<AudioManager>().Play("TaskStarted");
        taskEnteredAudio.GetComponent<AudioPlayer>().PlayAudio();
        isTaskInProgress = true;
    }

    public bool IsAstroSpecialist()
    {
        return isAstroSpecialist;
    }

    public bool IsTaskInProgress()
    {
        return isTaskInProgress;
    }

    public void SetAstroSpecialist(bool isAstroSpecialist)
    {
        this.isAstroSpecialist = isAstroSpecialist;
    }

    public void SetTaskInProgress(bool isTaskInProgress)
    {
        this.isTaskInProgress = isTaskInProgress;
    }

    public String GetTaskName()
    {
        return taskName;
    }
}
