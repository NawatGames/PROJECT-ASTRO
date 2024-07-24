using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TasksManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    private List<TaskController> _tasksForThisLevel;
    private List<TaskController> _tasksNotYetSelected;
    private Dictionary<TaskController, Coroutine> _taskQueue;
    private int maxNumberOfActiveTasks;
    [SerializeField] private int totalTimeForTaskToFail = 90;
    [SerializeField] private int shortTimeForTaskToBeCompleted = 30;
    [SerializeField] private GameEvent onTaskFailed;

    private void Start()
    {
        _taskQueue = new Dictionary<TaskController, Coroutine>();
        maxNumberOfActiveTasks = levelManager.getMaxNumberOfActiveTasks();
        _tasksForThisLevel = levelManager.GetTasksForThisLevel();
        _tasksNotYetSelected =  new List<TaskController>(_tasksForThisLevel);
        SetupStartingTasks();
    }

    private void SetupStartingTasks()
    {
        for (var i = 0; i < maxNumberOfActiveTasks; i++)
        {
            AddTaskToQueue();
        }
    }

    private TaskController SelectNextTask()
    {
        var randomTaskNumber = Random.Range(0, _tasksNotYetSelected.Count);
        var task = _tasksNotYetSelected[randomTaskNumber];
        _tasksNotYetSelected.Remove(task);
        if (_tasksNotYetSelected.Count == 0)
        {
            _tasksNotYetSelected = new List<TaskController>(_tasksForThisLevel);
        }
        return task;
    }

    private void AddTaskToQueue()
    {
        var task = SelectNextTask();
        _taskQueue.Add(task, StartCoroutine(TaskTimer(task)));
    }

    public void TaskDoneSuccessfully(TaskController task)
    {
        task.Mistakes = 0;
        StopCoroutine(_taskQueue[task]);
        _taskQueue.Remove(task);
        task.needsToBeDone = false;
        AddTaskToQueue();
    }

    public void KickPlayer(TaskController task)
    {
        StartCoroutine(KickPlayerRoutine(task));
    }
    
    private IEnumerator KickPlayerRoutine(TaskController task)
    {
        task.needsToBeDone = false;
        yield return null;
        task.needsToBeDone = true;
    }

    private IEnumerator TaskTimer(TaskController task)
    {
        yield return null; // Para dar tempo do needsToBeDone == false ser lido
        task.needsToBeDone = true;
        yield return new WaitForSecondsRealtime(totalTimeForTaskToFail - shortTimeForTaskToBeCompleted);
        // MOSTRAR AVISO DE TEMPO ACABANDO AQUI
        Debug.Log($"{task.taskScript} is running out of time!");
        yield return new WaitForSecondsRealtime(shortTimeForTaskToBeCompleted);
        task.Mistakes = 0;
        onTaskFailed.Raise();
        _taskQueue.Remove(task);
        task.needsToBeDone = false;
        AddTaskToQueue();
    }
}
