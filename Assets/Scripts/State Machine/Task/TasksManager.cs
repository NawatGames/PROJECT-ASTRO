using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    private List<TaskController> _tasksForThisLevel;
    private List<TaskController> _tasksNotYetSelected;
    private Dictionary<TaskController, IEnumerator> _taskQueue;
    [SerializeField] private int numberOfStartingTasks;
    [SerializeField] private int totalTimeForTaskToFail;
    [SerializeField] private int shortTimeForTaskToBeCompleted;
    [SerializeField] private GameEvent onTaskAddedToQueue;
    [SerializeField] private GameEvent onTaskRemovedFromQueue;
    [SerializeField] private GameEvent onShortTimeToCompleteTask;
    [SerializeField] private GameEvent onTaskFailed;

    private void Start()
    {
        _tasksForThisLevel = levelManager.GetTasksForThisLevel();
        _tasksNotYetSelected = _tasksForThisLevel;
        SetupStartingTasks();
        StartCoroutine(KeepAddingTasks());
    }

    private void SetupStartingTasks()
    {
        for (var i = 0; i < numberOfStartingTasks; i++)
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
            _tasksNotYetSelected = _tasksForThisLevel;
        }
        return task;
    }

    private void AddTaskToQueue()
    {
        var task = SelectNextTask();
        task.needsToBeDone = true;
        _taskQueue.Add(task, null);
        onTaskAddedToQueue.Raise();
    }

    public void TaskDoneSuccessfully(TaskController taskCtrl)
    {
        var coroutine = _taskQueue[taskCtrl];
        StopCoroutine(coroutine);
        _taskQueue.Remove(taskCtrl);
        onTaskRemovedFromQueue.Raise();
    }

    private void SetTaskTimer()
    {
        StartCoroutine(TaskTimer());
    }

    private IEnumerator TaskTimer()
    {
        yield return new WaitForSecondsRealtime(totalTimeForTaskToFail);
        onTaskFailed.Raise();
    }
    
    private IEnumerator KeepAddingTasks()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(25, 40));
            AddTaskToQueue();
        }
    }
}
