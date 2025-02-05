using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTester : MonoBehaviour
{
    [SerializeField] private GameOverManager _gameOver;
    [SerializeField] private bool _isActive;

    void Update()
    {
        if(_isActive)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                _gameOver.StartGameOver();
            }
        }
    }
}
