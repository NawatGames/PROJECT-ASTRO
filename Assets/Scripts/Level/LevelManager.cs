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
        CheckTaskNumbers();
        
        fadeImage.gameObject.SetActive(false);

        _levelIndex = SaveManager.CurrentLevel - 1;

        _thisLevelTasks = new List<TaskController>();
        foreach (TaskSO taskSO in levelParams[_levelIndex].tasks)
        {
            _thisLevelTasks.Add(GameObject.Find(taskSO.taskControllerCarrierName).GetComponent<TaskController>());
        }
    }

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
            SceneManager.LoadScene("Winscreen");
        }
    }
}
