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
    
    private static int _currentLevel;
    private static bool _isLoaded;

    // Carrega o save na primeira leitura, para não depender da ordem dos Awake entre cenas/objetos
    public static int CurrentLevel
    {
        get
        {
            if (!_isLoaded)
            {
                _currentLevel = LoadSaveFile();
                _isLoaded = true;
            }
            return _currentLevel;
        }
        private set
        {
            _currentLevel = value;
            _isLoaded = true;
        }
    }

    private static string FilePath => Path.Combine(Application.persistentDataPath, "saveData.json");

    private void Awake()
    {
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
        Debug.Log("path:   " + FilePath);
        File.WriteAllText(FilePath, JsonUtility.ToJson(new SaveData(level)));
        Debug.Log("Value saved: " + level);
    }

    private static int LoadSaveFile()
    {
        if(File.Exists(FilePath))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
            Debug.Log("LOAD: " + saveData.data);
            return saveData.data;
        }
        return 0;
    }
}
