using System.Collections;
using System.Collections.Generic;
using Audio_System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour
{

    [Header("Parameters")]
    private int _levelIndex;
    [SerializeField] private LevelParameters[] levelParams;
    private List<List<TaskController>> _allLevelTasks;
    [SerializeField] private List<TaskController> level0Tasks;
    [SerializeField] private List<TaskController> level1Tasks;
    [SerializeField] private List<TaskController> level2Tasks;
    [SerializeField] private List<TaskController> level3Tasks;
    [SerializeField] private List<TaskController> level4Tasks;
    
    private void Awake()
    {
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
    
    public int GetMaxNumberOfActiveTasks()
    {
        return levelParams[_levelIndex].maxActiveTasks;
    }
    
    public List<TaskController> GetTasksForThisLevel()
    {
        return _allLevelTasks[_levelIndex];
    }

    public void NextLevel() // Chamado por evento
    {
        Debug.Log("passou");
        SaveManager.IncreaseLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
