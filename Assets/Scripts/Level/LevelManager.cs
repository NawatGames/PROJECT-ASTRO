using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class LevelManager : MonoBehaviour
{

    [Header("Parameters")]
    private int _levelIndex;
    public LevelParameters[] levelParams; // TODO: Tornar essa variavel sem ser lista. Criar uma variavel serialized private para ser a lista e, baseado nela + levelIndex, preencher esta (public, NÃO LISTA)
    private List<TaskController> _thisLevelTasks;
    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        // Clamp evita índice inválido ao abrir a cena sem save (CurrentLevel = 0) ou com save de mais fases
        _levelIndex = Mathf.Clamp(SaveManager.CurrentLevel, 1, levelParams.Length) - 1;

        CheckTaskNumbers();

        fadeImage.gameObject.SetActive(false);

        TaskController[] sceneTasks = FindObjectsOfType<TaskController>();
        _thisLevelTasks = new List<TaskController>();
        foreach (TaskSO taskSO in levelParams[_levelIndex].tasks)
        {
            TaskController task = FindTaskController(sceneTasks, taskSO);
            if (task == null)
            {
                Debug.LogError($"Nenhum TaskController em cena para o TaskSO '{taskSO.name}'");
                continue;
            }
            _thisLevelTasks.Add(task);
        }
    }

    // Prioriza a referência TaskController.taskSO; o nome do objeto fica só como fallback
    private static TaskController FindTaskController(TaskController[] sceneTasks, TaskSO taskSO)
    {
        foreach (TaskController task in sceneTasks)
        {
            if (task.taskSO == taskSO) return task;
        }
        foreach (TaskController task in sceneTasks)
        {
            if (task.gameObject.name == taskSO.taskControllerCarrierName)
            {
                Debug.LogWarning($"'{task.name}' encontrado pelo nome. Use 'Vincular TaskSOs pelo nome' no LevelManager e salve a cena.");
                return task;
            }
        }
        return null;
    }

#if UNITY_EDITOR
    // Preenche TaskController.taskSO uma vez, a partir dos nomes atuais; depois os objetos podem ser renomeados
    [ContextMenu("Vincular TaskSOs pelo nome")]
    private void LinkTaskSOsByName()
    {
        TaskController[] sceneTasks = FindObjectsOfType<TaskController>(true);
        var linked = new HashSet<TaskSO>();
        foreach (LevelParameters level in levelParams)
        {
            foreach (TaskSO taskSO in level.tasks)
            {
                if (!linked.Add(taskSO)) continue;
                foreach (TaskController task in sceneTasks)
                {
                    if (task.gameObject.name != taskSO.taskControllerCarrierName) continue;
                    UnityEditor.Undo.RecordObject(task, "Vincular TaskSO");
                    task.taskSO = taskSO;
                    UnityEditor.EditorUtility.SetDirty(task);
                    Debug.Log($"{task.name} -> {taskSO.name}");
                }
            }
        }
    }
#endif

    [Conditional("UNITY_EDITOR")]
    private void CheckTaskNumbers()
    {
        if (levelParams[_levelIndex].maxActiveTasks > levelParams[_levelIndex].tasks.Count)
        {
            Debug.LogError("ERRO NO SO LevelParameters desta fase (maxActiveTasks > tasks.Count)");
        }

        if (levelParams[_levelIndex].startingTasks > levelParams[_levelIndex].maxActiveTasks)
        {
            Debug.LogError("ERRO NO SO LevelParameters desta fase (startingTasks > maxActiveTasks)");
        }
    }

    #region Getters

    public LevelParameters GetLevelParameters()
    {
        return levelParams[_levelIndex];
    }

    public int GetMaxNumberOfActiveTasks()
    {
        return levelParams[_levelIndex].maxActiveTasks;
    }

    public List<TaskController> GetTasksForThisLevel()
    {
        return _thisLevelTasks;
    }

    public int GetStartingTasks()
    {
        return levelParams[_levelIndex].startingTasks;
    }

    public float GetTaskTimeWindow()
    {
        return levelParams[_levelIndex].taskTimeWindow;
    }

    public float GetTaskWarningTimeRatio()
    {
        return levelParams[_levelIndex].taskWarningTimeRatio;
    }
    
    #endregion
    
    [ContextMenu("LevelCompleted")]
    public void LevelCompleted()
    {
        StartCoroutine(FadeOutWin());
    }
    IEnumerator FadeOutWin()
    {
        fadeImage.gameObject.SetActive(true);
        float fadeDuration = 1;

        float elapsedTime = 0f;
        Color color = fadeImage.color;

        color.a = 0f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            color.a = elapsedTime / fadeDuration;
            fadeImage.color = color;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
        yield return new WaitForSeconds(1f);
        if (SaveManager.CurrentLevel < levelParams.Length) // Se ainda tiver fases pra passar
        {
            SaveManager.IncreaseLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(SceneNames.WinScreen);
        }
    }
}
