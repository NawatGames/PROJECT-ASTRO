using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTester : MonoBehaviour
{
    public GameOverManager gameOver;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            gameOver.StartGameOver();
        }
    }
}
