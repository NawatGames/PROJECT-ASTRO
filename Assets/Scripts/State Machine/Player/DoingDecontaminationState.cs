using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoingDecontaminationState : IPlayerState
{
    public void Enter(PlayerController player) 
    {
        player.isDoingDecontamination = true;
        player.startedDecontaminationEvent.Raise();
    }
    public IPlayerState Do(PlayerController player)
    {
        if(player.InteractAction.WasPressedThisFrame() || !player.isDoingDecontamination)
        {
            player.stoppedDecontaminationEvent.Raise();
            return player.FreeMovingState;
        }
        if (player.GameIsOver)
        {
            return player.GameOverState;
        }
        return player.DoingDecontaminationState;
    }
    public void Exit(PlayerController player)
    {

    }



}