using System;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    private class SaveData
    {
        public int data;
        public SaveData(int level)
        {
            this.data = level;
        }
    }
    
    public static int CurrentLevel { get; private set; }
    private static string _filePath;

    private void Awake()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        //Debug.Log("Caminho do arquivo:" + _filePath);
        CurrentLevel = LoadSaveFile();
        ////// RESETAR P LEVEL 1 :
        //CurrentLevel = 1;
        //////
        Debug.Log("Nível: " + CurrentLevel);
    }

    public static void IncreaseLevel()
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
        Debug.Log("path:   " + _filePath);
        File.WriteAllText(_filePath, JsonUtility.ToJson(new SaveData(level)));
        Debug.Log("Value saved: " + level);
    }

    private static int LoadSaveFile()
    {
        if(File.Exists(_filePath))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_filePath));
            Debug.Log("LOAD: " + saveData.data);
            return saveData.data;
        }
        return 0;
    }
}
