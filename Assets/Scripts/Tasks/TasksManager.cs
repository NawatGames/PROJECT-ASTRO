using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int totalTimeForTaskToFail = 90;
    [SerializeField] private int shortTimeForTaskToBeCompleted = 30;
    [SerializeField] private GameEvent onTaskFailed;
    [SerializeField] private GameObject taskTimerPrefab;
    [SerializeField] private Transform taskGridLayoutTransform;
    private List<TaskController> _tasksForThisLevel;
    [SerializeField] private List<TaskController> _tasksNotYetSelected;
    private Dictionary<TaskController, Coroutine> _taskQueue;
    private int maxNumberOfActiveTasks;
    private TaskController recentRemovedTask;

    private void Start()
    {
        _taskQueue = new Dictionary<TaskController, Coroutine>();
        maxNumberOfActiveTasks = levelManager.GetMaxNumberOfActiveTasks();
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
        TaskController task;
        if (_tasksNotYetSelected.Count == 0)
        {
            task = recentRemovedTask;
        }
        else
        {
            var randomTaskNumber = Random.Range(0, _tasksNotYetSelected.Count);
            task = _tasksNotYetSelected[randomTaskNumber];
        }
        
        _tasksNotYetSelected.Remove(task);
        _taskQueue.Add(task, StartCoroutine(StartTaskTimer(task)));
        if (_tasksNotYetSelected.Count == 0)
        {
            List<TaskController> resetList = new List<TaskController>(_tasksForThisLevel);
            foreach (TaskController t in _taskQueue.Keys)
            {
                //Debug.Log("ja na fila:  " + t);
                resetList.Remove(t);
            }
            _tasksNotYetSelected = resetList;
        }
        return task;
    }

    private void AddTaskToQueue()
    {
        var task = SelectNextTask();
    }

    public void TaskDoneSuccessfully(TaskController task)
    {
        StopCoroutine(_taskQueue[task]);
        RemoveTaskFromQueue(task);
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

    private IEnumerator StartTaskTimer(TaskController task)
    {
        yield return new WaitForSeconds(Random.Range(1, 4.5f)); // Tempo para habilitar nova task
        task.needsToBeDone = true;
        TextMeshProUGUI taskTimerTMP = Instantiate(taskTimerPrefab, taskGridLayoutTransform).GetComponent<TextMeshProUGUI>();
        int min = totalTimeForTaskToFail / 60;
        int sec = totalTimeForTaskToFail - 60 * min;
        int minF = shortTimeForTaskToBeCompleted / 60;
        int secF = shortTimeForTaskToBeCompleted - 60 * min;
        taskTimerTMP.text = $"{task.taskName}: {min,2}:{sec:00}";
        while (sec > secF || min > minF)
        {
            yield return new WaitForSecondsRealtime(1);
            sec--;
            if (sec < 0)
            {
                min--;
                sec = 59;
            }
            taskTimerTMP.text = $"{task.taskName}: {min,2}:{sec:00}";
        }
        StartCoroutine(TaskShortTime(task, taskTimerTMP));
    }

    private IEnumerator TaskShortTime(TaskController task, TextMeshProUGUI taskTimerTMP)
    {
        // MOSTRAR AVISO DE TEMPO ACABANDO AQUI
        //Debug.Log($"{task.taskScript} is running out of time!");
        int min = shortTimeForTaskToBeCompleted / 60;
        int sec = shortTimeForTaskToBeCompleted - 60 * min;
        taskTimerTMP.text = $"{task.taskName}: {min,2}:{sec:00}";
        while (sec > 0 || min > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            sec--;
            if (sec < 0)
            {
                min--;
                sec = 59;
            }
            taskTimerTMP.text = $"{task.taskName}: {min,2}:{sec:00}";
        }
        TaskTimedOut(task, taskTimerTMP);
    }

    private void TaskTimedOut(TaskController task, TextMeshProUGUI taskTimerTMP)
    {
        Destroy(taskTimerTMP.gameObject);
        onTaskFailed.Raise();
        RemoveTaskFromQueue(task);
        
    }

    private void RemoveTaskFromQueue(TaskController task)
    {
        recentRemovedTask = task;
        task.Mistakes = 0;
        _taskQueue.Remove(task);
        task.needsToBeDone = false;
        AddTaskToQueue();
    }
}
