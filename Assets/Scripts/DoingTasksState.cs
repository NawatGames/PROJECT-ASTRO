using System;
using UnityEngine;

public class DoingTasksState : IPlayerState
{
    public IPlayerState Do(PlayerController player)
    {
        if (Input.GetKeyDown(player.InteractKey))
        {
            Debug.Log("Parando de fazer task");
            return player.FreeMovingState;
        }

        return player.DoingTasksState;
    }
}