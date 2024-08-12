using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButton : MonoBehaviour
{
    public int buttonType;
    public GameObject LevelSaver;

    private SaveManager SaveManager;
    private int _counter;

    void Awake()
    {
        SaveManager = LevelSaver.GetComponent<SaveManager>();
    }

    public void OnButtonClick()
    {

        switch(buttonType)
        {
            case 1:
                _counter  = 0;
                Debug.Log("Overwriting previous save progress...");
                SaveManager.SaveData(1);
                break;
            
            case 2:
                _counter = SaveManager.LoadData();
                Debug.Log(_counter);
                break;
            
            case 3:
                _counter = SaveManager.LoadData();
                _counter++;
                SaveManager.SaveData(_counter);
                break;
        }
    }
}
