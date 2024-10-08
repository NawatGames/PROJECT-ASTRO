using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskPlayerState : PlayerState
{
    [SerializeField] private PlayerCollisionController playerCollisionController;
    
    public override void EnterState()
    {
        base.EnterState();
        playerCollisionController.NearTaskController.wasStarted = true;
        playerCollisionController.NearTaskController.taskScript.SetupAndRun(playerInputController.inputAsset, playerStateMachine.isAstro);
    }

    public override void StateUpdate()
    {
        // TODO: "Task parou" (needsToBeDone <- false) ocorre em mts casos diferentes. Separar, sem usar essa var 
        if (!playerCollisionController.NearTaskController.needsToBeDone)
        {
            Debug.Log("Task parou");
            SwitchState(playerStateMachine.freeMoveState);
        }
        else if (playerStateMachine.GameIsOver)
        {
            SwitchState(playerStateMachine.gameOverState);
        }
    }
    
    protected override void OnInteractHandler(InputAction.CallbackContext ctx)
    {
        Debug.Log("Player saiu da task");
        playerCollisionController.NearTaskController.wasInterrupted = true;
        SwitchState(playerStateMachine.freeMoveState);
    }
}
