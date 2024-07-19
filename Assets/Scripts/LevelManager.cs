using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int _level;
    private List<List<TaskController>> _allLevelTasks;
    [SerializeField] private List<TaskController> level0Tasks;
    [SerializeField] private List<TaskController> level1Tasks;
    [SerializeField] private List<TaskController> level2Tasks;
    [SerializeField] private List<TaskController> level3Tasks;
    [SerializeField] private List<TaskController> level4Tasks;
    [SerializeField] private int countdownTillVictory;
    [SerializeField] private GameEvent onVictory;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (PlayerPrefs.HasKey("level"))
        {
            _level = PlayerPrefs.GetInt("level");
        }
        _allLevelTasks.Add(level0Tasks);
        _allLevelTasks.Add(level1Tasks);
        _allLevelTasks.Add(level2Tasks);
        _allLevelTasks.Add(level3Tasks);
        _allLevelTasks.Add(level4Tasks);
    }

    private void Start()
    {
        StartCoroutine(CountdownTillVictory());
    }

    public List<TaskController> GetTasksForThisLevel()
    {
        return _allLevelTasks[_level];
    }

    public void NextLevel()
    {
        _level += 1;
        PlayerPrefs.SetInt("level", _level);
    }

    public void GameOver()
    {
        StopCoroutine(CountdownTillVictory());
    }   

    private IEnumerator CountdownTillVictory()
    {
        yield return new WaitForSecondsRealtime(countdownTillVictory);
        onVictory.Raise();
    }
}
