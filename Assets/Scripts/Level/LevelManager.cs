using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    [Header("Parameters")]
    private int _levelIndex;
    public LevelParameters[] levelParams;
    private List<List<TaskController>> _allLevelTasks;
    [SerializeField] private List<TaskController> level0Tasks;
    [SerializeField] private List<TaskController> level1Tasks;
    [SerializeField] private List<TaskController> level2Tasks;
    [SerializeField] private List<TaskController> level3Tasks;
    [SerializeField] private List<TaskController> level4Tasks;
    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        fadeImage.gameObject.SetActive(false);

        _levelIndex = SaveManager.CurrentLevel - 1;

        _allLevelTasks = new List<List<TaskController>>
        {
            level0Tasks,
            level1Tasks,
            level2Tasks,
            level3Tasks,
            level4Tasks
        };

    }

    #region Getters
    
    public int GetMaxNumberOfActiveTasks()
    {
        return levelParams[_levelIndex].maxActiveTasks;
    }

    public List<TaskController> GetTasksForThisLevel()
    {
        return _allLevelTasks[_levelIndex];
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
