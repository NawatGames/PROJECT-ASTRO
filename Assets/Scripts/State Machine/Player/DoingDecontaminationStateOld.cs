using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoingDecontaminationStateOld : IPlayerState
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
            return player.FreeMovingStateOld;
        }
        if (player.GameIsOver)
        {
            return player.GameOverStateOld;
        }
        return player.DoingDecontaminationStateOld;
    }
    public void Exit(PlayerController player)
    {

    }



}