using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour
{

    public int level;
    public bool levelLocked = false;

    void Start()
    {
        if(level > PlayerPrefs.GetInt("levelsUnlocked"))
        {
            levelLocked = true;
        }
    }
    
    public void OnButtonClick()
    {
        if(levelLocked == false)
        {
            Debug.Log("Starting level " + level.ToString() + "...");
        }
    }
}
