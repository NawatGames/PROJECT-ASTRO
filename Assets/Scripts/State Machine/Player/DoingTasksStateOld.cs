using UnityEngine;

public class DoingTasksStateOld : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.NearTaskController.wasStarted = true;
        player.NearTaskController.taskScript.SetupAndRun(player.Input, player.isAstro);
    }
    public IPlayerState Do(PlayerController player)
    {
        if (!player.NearTaskController.needsToBeDone)
        {
            Debug.Log("Task parou");
            return player.FreeMovingStateOld;
        }
        if (player.InteractAction.WasPressedThisFrame())
        {
            Debug.Log("Parando de fazer task");
            player.NearTaskController.wasInterrupted = true;
            return player.FreeMovingStateOld;
        }
        if (player.GameIsOver)
        {
            return player.GameOverStateOld;
        }
        return player.DoingTasksStateOld;
    }
}