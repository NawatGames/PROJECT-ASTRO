using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    [Space]
    [SerializeField] private int totalTimeForTaskToFail = 90;
    [SerializeField] private int shortTimeForTaskToBeCompleted = 30;
    [SerializeField] private int nextTaskMinDelay = 5;
    [SerializeField] private int nextTaskMaxDelay = 10;
    [SerializeField] private int startingTasks = 3;
    
    [Space]
    [SerializeField] private int astroProbability = 50;

    [Space]
    [SerializeField] private GameEvent onTaskFailed;
    [SerializeField] private GameObject taskTimerPrefab;
    [SerializeField] private Transform taskGridLayoutTransform;
    
    private List<TaskController> _tasksNotYetSelected;
    private List<TaskController> _tasksForThisLevel;
    private Dictionary<TaskController, Coroutine> _taskQueue;
    private TaskController _recentRemovedTask;

    private bool _hasOneStartingAlienSpecialist;
    private bool _hasOneStartingAstroSpecialist;
    private bool _forceOneStartingSpecialist;

    private void Start()    
    {
        _taskQueue = new Dictionary<TaskController, Coroutine>();
        _tasksForThisLevel = levelManager.GetTasksForThisLevel();
        _tasksNotYetSelected =  new List<TaskController>(_tasksForThisLevel);
        StartCoroutine(SetupStartingTasks());
    }

    private IEnumerator SetupStartingTasks()
    {
        // Adiciona as primeiras (-1) tasks
        for (var i = 1; i <= startingTasks - 1; i++)
        {
            AddNextTaskToQueue();
        }
        if (!_hasOneStartingAstroSpecialist || !_hasOneStartingAlienSpecialist)
        {
            _forceOneStartingSpecialist = true;
        }
        AddNextTaskToQueue();
        
        // Adiciona as próximas tasks
        yield return StartCoroutine(WaitAndAddTaskToQueue(levelManager.GetMaxNumberOfActiveTasks() - startingTasks));
    }

    private TaskController AddNextTaskToQueue()
    {
        TaskController task;
        if (_tasksNotYetSelected.Count == 0)
        {
            task = _recentRemovedTask;
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
                resetList.Remove(t);
            }
            _tasksNotYetSelected = resetList;
        }
        return task;
    }

    private IEnumerator WaitAndAddTaskToQueue()
    {
        yield return new WaitForSeconds(Random.Range(nextTaskMinDelay, nextTaskMaxDelay));
        AddNextTaskToQueue();
    }

    private IEnumerator WaitAndAddTaskToQueue(int tasks)
    {
        if (tasks == 0) yield break;
        yield return new WaitForSeconds(Random.Range(nextTaskMinDelay, nextTaskMaxDelay));
        AddNextTaskToQueue();
        StartCoroutine(WaitAndAddTaskToQueue(tasks - 1));
    }

    public void TaskDoneSuccessfully(TaskController task)
    {
        task.StatusLight.TurnOnSuccess();
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
        task.needsToBeDone = true;
        task.brokenTaskMask?.SetActive(true);

        DefineSpecialist(task.taskScript);
        
        TextMeshProUGUI taskTimerTMP = Instantiate(taskTimerPrefab, taskGridLayoutTransform).GetComponent<TextMeshProUGUI>();
        float timeLeft = totalTimeForTaskToFail;
        int minutes = totalTimeForTaskToFail / 60;
        int seconds = totalTimeForTaskToFail - 60 * minutes;
        taskTimerTMP.text = $"{task.taskName}: {minutes,2}:{seconds:00}";
        
        if (task.taskScript.IsAstroSpecialist()) task.StatusLight.TurnOnAstro();
        else task.StatusLight.TurnOnOrion();
        
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
        float timeLeft = shortTimeForTaskToBeCompleted;
        int minutes = shortTimeForTaskToBeCompleted / 60;
        int seconds = shortTimeForTaskToBeCompleted - 60 * minutes;
        taskTimerTMP.text = $"{task.taskName}: {minutes,2}:{seconds:00}";
        
        task.StatusLight.TurnOnWarning();
        // task.StatusLight.Blink())
        
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
        task.StatusLight.TurnOnFailed();
        Destroy(taskTimerTMP.gameObject);
        onTaskFailed.Raise();
        RemoveTaskFromQueue(task);
    }

    private void RemoveTaskFromQueue(TaskController task)
    {
        _recentRemovedTask = task;
        task.Mistakes = 0;
        _taskQueue.Remove(task);
        task.needsToBeDone = false;
        task.brokenTaskMask?.SetActive(false);
        StartCoroutine(WaitAndAddTaskToQueue());
    }

    private void DefineSpecialist(TaskScript taskScript)
    {
        int specialistRng;
        if (_forceOneStartingSpecialist)
        {
            _forceOneStartingSpecialist = false;
            specialistRng = _hasOneStartingAlienSpecialist ? astroProbability : astroProbability + 1;
        }
        else
        {
            specialistRng = Random.Range(1, 101);
        }
        
        if(specialistRng <= astroProbability)
        {
            _hasOneStartingAstroSpecialist = true;
            taskScript.SetAstroSpecialist(true);
        }
        else
        {
            _hasOneStartingAlienSpecialist = true;
            taskScript.SetAstroSpecialist(false);
        }
    }
}
