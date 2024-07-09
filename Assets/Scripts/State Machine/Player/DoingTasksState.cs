using UnityEngine;

public class DoingTasksState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.NearTaskController.taskScript.SetupAndRun(player.input.asset, player.isAstro);
    }
    public IPlayerState Do(PlayerController player)
    {
        if (player.InteractAction.WasPressedThisFrame())
        {
            Debug.Log("Parando de fazer task");
            player.NearTaskController.wasInterrupted = true;
            return player.FreeMovingState;
        }
        return player.DoingTasksState;
    }

    public void FixedDo(PlayerController player)
    {
        
    }

    public void Exit(PlayerController player)
    {
        player.NearTaskController.taskScript.RemoveInput();
    }
}