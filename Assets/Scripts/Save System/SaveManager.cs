using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static int CurrentLevel { get; private set; }
    private static string _filePath;
    
    private void Awake()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log("Caminho do arquivo:" + _filePath);
        CurrentLevel = LoadSaveFile();
        Debug.Log("Nível: " + CurrentLevel);
    }

    public void NextLevel()
    {
        CurrentLevel += 1;
        SaveLevelData(CurrentLevel);
    }

    public static void CreateNewSaveFile()
    {
        CurrentLevel = 1;
        SaveLevelData(CurrentLevel);
    }

    private static void SaveLevelData(int level)
    {
        File.WriteAllText(_filePath, JsonUtility.ToJson(level));
        Debug.Log("Value saved: " + level);
    }

    private static int LoadSaveFile()
    {
        if(File.Exists(_filePath))
        {
            int level = JsonUtility.FromJson<int>(File.ReadAllText(_filePath));
            return level;
        }
        return 0;
    }
}
