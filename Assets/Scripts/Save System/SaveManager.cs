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
        
        ////// RESETAR P LEVEL 1 :
        //SaveLevelData(1);
        //////
        
        CurrentLevel = LoadSaveFile();
        
        //Debug.Log("Nível: " + CurrentLevel);
    }

#if UNITY_EDITOR
    [ContextMenu("Reset Save File")]
    public void ResetSaveFile()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("O jogo precisa estar rodando pra chamar essa funcao");
        }
        else
        {
            SaveLevelData(1);
        }
    }
#endif
    
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
