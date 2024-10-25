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
    [SerializeField] private int astroProbability = 50;

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
        task.StatusLight.TurnOff();
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
        DefineSpecialist(task.taskScript);
        
        TextMeshProUGUI taskTimerTMP = Instantiate(taskTimerPrefab, taskGridLayoutTransform).GetComponent<TextMeshProUGUI>();
        float timeLeft = totalTimeForTaskToFail;
        int minutes = totalTimeForTaskToFail / 60;
        int seconds = totalTimeForTaskToFail - 60 * minutes;
        taskTimerTMP.text = $"{task.taskName}: {minutes,2}:{seconds:00}";
        
        if (task.taskScript.IsAstroSpecialist()) task.StatusLight.TurnOnAstro();
        else task.StatusLight.TurnOnAlien();
        
        while (timeLeft > shortTimeForTaskToBeCompleted)
        {
            yield return new WaitUntil(() => task.taskScript.IsTaskInProgress() == false);
            
            timeLeft -= Time.deltaTime;
            minutes = Mathf.FloorToInt(timeLeft/60);
            seconds = Mathf.FloorToInt(timeLeft%60);
            taskTimerTMP.text = $"{task.taskName}: {minutes,2}:{seconds:00}";
        }
        StartCoroutine(TaskShortTime(task, taskTimerTMP));
    }

    private IEnumerator TaskShortTime(TaskController task, TextMeshProUGUI taskTimerTMP)
    {
        // Debug.Log($"{task.taskScript} is running out of time!");
        float timeLeft = shortTimeForTaskToBeCompleted;
        int minutes = shortTimeForTaskToBeCompleted / 60;
        int seconds = shortTimeForTaskToBeCompleted - 60 * minutes;
        taskTimerTMP.text = $"{task.taskName}: {minutes,2}:{seconds:00}";
        
        task.StatusLight.TurnOnWarning();
        
        while (timeLeft > 0)
        {
            yield return new WaitUntil(() => task.taskScript.IsTaskInProgress() == false);
            
            timeLeft -= Time.deltaTime;
            minutes = Mathf.FloorToInt(timeLeft/60);
            seconds = Mathf.FloorToInt(timeLeft%60);
            taskTimerTMP.text = $"{task.taskName}: {minutes,2}:{seconds:00}";
        }
        TaskTimedOut(task, taskTimerTMP);
    }

    private void TaskTimedOut(TaskController task, TextMeshProUGUI taskTimerTMP)
    {
        StartCoroutine(task.StatusLight.Blink(Color.red, 4, 0.2f));
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

    private void DefineSpecialist(TaskScript taskScript)
    {
        int specialistRng;
        specialistRng = Random.Range(1, 101);
        if(specialistRng >= astroProbability)
        {
            //Debug.Log(specialistRng);
            //Debug.Log("Astro is the specialist of the " + taskScript.GetTaskName());
            taskScript.SetAstroSpecialist(true);
        }

        else
        {
            //Debug.Log(specialistRng);
            //Debug.Log("Astro is not the specialist of the " + taskScript.GetTaskName());
            taskScript.SetAstroSpecialist(false);
        }
    }
}
