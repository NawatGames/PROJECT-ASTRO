using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    [Space]
    [SerializeField] private int nextTaskMinDelay = 5;
    [SerializeField] private int nextTaskMaxDelay = 10;

    [Space]
    [SerializeField] private int astroProbability = 50;

    [Space]
    [SerializeField] private GameEvent onTaskFailed;
    [SerializeField] private GameObject taskTimerPrefab;
    [SerializeField] private Transform taskGridLayoutTransform;

    private List<TaskController> _tasksNotYetSelected;
    private List<TaskController> _tasksForThisLevel;
    private Dictionary<TaskController, ActiveTask> _taskQueue;

    // Timer e texto de UI de uma task ativa (removidos juntos ao sair da fila)
    private class ActiveTask
    {
        public Coroutine timer;
        public TextMeshProUGUI timerText;
    }
    private TaskController _recentRemovedTask;
    
    private float _taskTimeWindow;
    private float _taskWarningTimeWindow;
    
    private bool _hasOneStartingAlienSpecialist;
    private bool _hasOneStartingAstroSpecialist;
    private bool _forceOneStartingSpecialist;

    private void Start()
    {
        _taskTimeWindow = levelManager.GetTaskTimeWindow();
        _taskWarningTimeWindow = _taskTimeWindow * levelManager.GetTaskWarningTimeRatio();
        _taskQueue = new Dictionary<TaskController, ActiveTask>();
        _tasksForThisLevel = levelManager.GetTasksForThisLevel();
        _tasksNotYetSelected = new List<TaskController>(_tasksForThisLevel);
        StartCoroutine(SetupStartingTasks());
    }

    private IEnumerator SetupStartingTasks()
    {
        // Adiciona as primeiras (-1) tasks
        int startingTasks = levelManager.GetStartingTasks();
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
        var activeTask = new ActiveTask();
        _taskQueue.Add(task, activeTask);
        activeTask.timer = StartCoroutine(StartTaskTimer(task, activeTask));

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
        if (!_taskQueue.TryGetValue(task, out ActiveTask activeTask)) return; // Já saiu da fila (sucesso duplicado)

        task.StatusLight.TurnOnSuccess();
        StopCoroutine(activeTask.timer);
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

    // Uma única coroutine por task (tempo normal + warning), para que TaskDoneSuccessfully consiga pará-la
    private IEnumerator StartTaskTimer(TaskController task, ActiveTask activeTask)
    {
        task.needsToBeDone = true;
        task.taskVisual?.SetIsBroken(true);

        DefineSpecialist(task.taskScript);

        activeTask.timerText = Instantiate(taskTimerPrefab, taskGridLayoutTransform).GetComponent<TextMeshProUGUI>();
        float timeLeft = _taskTimeWindow;
        UpdateTimerText(task, activeTask.timerText, timeLeft);

        if (task.taskScript.IsAstroSpecialist()) task.StatusLight.TurnOnAstro();
        else task.StatusLight.TurnOnOrion();

        bool isOnWarning = false;
        while (timeLeft > 0)
        {
            yield return new WaitUntil(() => task.taskScript.IsTaskInProgress() == false);

            timeLeft -= Time.deltaTime;
            if (!isOnWarning && timeLeft <= _taskWarningTimeWindow)
            {
                isOnWarning = true;
                task.StatusLight.TurnOnWarning();
            }
            UpdateTimerText(task, activeTask.timerText, timeLeft);
        }
        TaskTimedOut(task);
    }

    private void UpdateTimerText(TaskController task, TextMeshProUGUI taskTimerTMP, float timeLeft)
    {
        timeLeft = Mathf.Max(timeLeft, 0);
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        taskTimerTMP.text = $"{task.taskName}: {minutes,2}:{seconds:00}";
    }

    private void TaskTimedOut(TaskController task)
    {
        task.StatusLight.TurnOnFailed();
        onTaskFailed.Raise();
        RemoveTaskFromQueue(task);
    }

    private void RemoveTaskFromQueue(TaskController task)
    {
        _recentRemovedTask = task;
        task.Mistakes = 0;
        if (_taskQueue.TryGetValue(task, out ActiveTask activeTask) && activeTask.timerText != null)
        {
            Destroy(activeTask.timerText.gameObject);
        }
        _taskQueue.Remove(task);
        task.needsToBeDone = false;
        task.taskVisual?.SetIsBroken(false);
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

        if (specialistRng <= astroProbability)
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
