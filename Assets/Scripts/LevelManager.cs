using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour
{
    private static bool _alreadyInstanced;
    public static int level;
    [SerializeField] private int[] maxActiveTasksPerLevel;
    private List<List<TaskController>> _allLevelTasks;
    [SerializeField] private List<TaskController> level0Tasks;
    [SerializeField] private List<TaskController> level1Tasks;
    [SerializeField] private List<TaskController> level2Tasks;
    [SerializeField] private List<TaskController> level3Tasks;
    [SerializeField] private List<TaskController> level4Tasks;
    
    private void Awake()
    {
        if (!_alreadyInstanced)
        {
            _alreadyInstanced = true;
            level = 0;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        /*if (PlayerPrefs.HasKey("level"))
        {
            _level = PlayerPrefs.GetInt("level");
        } FAZER LOAD*/

        _allLevelTasks = new List<List<TaskController>>
        {
            level0Tasks,
            level1Tasks,
            level2Tasks,
            level3Tasks,
            level4Tasks
        };
    }

    public int getMaxNumberOfActiveTasks()
    {
        return maxActiveTasksPerLevel[level];
    }
    
    public List<TaskController> GetTasksForThisLevel()
    {
        Debug.Log("Level: " + level);
        return _allLevelTasks[level];
    }

    public void NextLevel() // Chamado por evento
    {
        level += 1;
        //PlayerPrefs.SetInt("level", _level); FAZER SAVE
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver() // Chamado por evento
    {
        Debug.Log("GameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }   
}
