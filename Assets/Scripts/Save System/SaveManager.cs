using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string _filePath;

    private void Start()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log(_filePath);
    }

    public void SaveData(int difficulty)
    {
        DataSaver dataSaver = new DataSaver();
        dataSaver.data = difficulty;
        File.WriteAllText(_filePath, JsonUtility.ToJson(dataSaver));
        Debug.Log("Value saved: " + difficulty);
    }

    public int LoadData()
    {
        if(File.Exists(_filePath))
        {
            DataSaver dataSaver = JsonUtility.FromJson<DataSaver>(File.ReadAllText(_filePath));
            Debug.Log("Value loaded: " + dataSaver.data);
            return dataSaver.data;
        }

        else
        {
            Debug.Log("Save file not found");
            return 1;
        }
    }
}
