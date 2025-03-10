using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTest : MonoBehaviour
{
    public GameObject pauseMenu;

    void Start()
    {
        Instantiate(pauseMenu, new Vector2(0f, 0f), Quaternion.identity);
        pauseMenu.SetActive(false);
    }
}
