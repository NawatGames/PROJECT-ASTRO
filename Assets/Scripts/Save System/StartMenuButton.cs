using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButton : MonoBehaviour
{
    public int buttonType;

    public void OnButtonClick()
    {
        switch(buttonType)
        {
            case 1:
                PlayerPrefs.SetInt("levelsUnlocked", 1);
                break;
            
            case 2:
                SceneManager.LoadScene("LoadMenu");
                break;
            
            case 3:
                PlayerPrefs.SetInt("levelsUnlocked", 2);
                break;
        }
    }
}
