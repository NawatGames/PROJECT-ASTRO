using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenuManager : MonoBehaviour
{
    public Text levelOneText;
    public Text levelTwoText;
    void Start()
    {
        //TODO: trocar playerprefs para json serialization
        switch(PlayerPrefs.GetInt("levelsUnlocked"))
        {
            case 1:
                levelOneText.text = "";
                levelTwoText.text = "LOCKED";
                break;
            case 2:
                levelOneText.text = "";
                levelTwoText.text = "";
                break;
        }
    }
}
