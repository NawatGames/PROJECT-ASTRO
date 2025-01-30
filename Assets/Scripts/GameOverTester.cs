using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTester : MonoBehaviour
{
    public GameObject gameOver;

    void Start()
    {
        Instantiate(gameOver, new Vector2(0f, 0f), Quaternion.identity);
        gameOver.SetActive(false);
    }
}
