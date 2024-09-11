using UnityEngine;

public class DoingTasksState : IPlayerState
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
            return player.FreeMovingState;
        }
        if (player.InteractAction.WasPressedThisFrame())
        {
            Debug.Log("Parando de fazer task");
            player.NearTaskController.wasInterrupted = true;
            return player.FreeMovingState;
        }
        if (player.GameIsOver)
        {
            return player.GameOverState;
        }
        return player.DoingTasksState;
    }
}