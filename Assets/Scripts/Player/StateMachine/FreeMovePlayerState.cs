using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeMovePlayerState : PlayerState
{
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private TaskPlayerState taskplayerstate;

    public override void EnterState()
    {
        base.EnterState();
        playerMovementController.enabled = true;
    }

    public override void StateUpdate()
    {
        if (playerStateMachine.GameIsOver)
        {
            SwitchState(playerStateMachine.gameOverState);
        }
    }
    
    protected override void OnInteractHandler(InputAction.CallbackContext ctx)
    {
        if(!taskplayerstate.IsOnCooldown)
        {
        if (playerCollisionController.IsOnTaskArea && playerCollisionController.NearTaskController.currentState is AvailableState)
        {
            //Debug.Log("(walk)Started task");
            SwitchState(playerStateMachine.goToTaskState);
        }
        }
        else if (playerCollisionController.IsOnLobbyArea)
        {
            SwitchState(playerStateMachine.decontaminateState);
        }
        else if (playerCollisionController.IsOnButtonArea)
        {
            playerCollisionController.NearDoorButtonController.ToggleDoor();
        }
    }

    public override void LeaveState()
    {
        base.LeaveState();
        playerMovementController.enabled = false;
    }
}
