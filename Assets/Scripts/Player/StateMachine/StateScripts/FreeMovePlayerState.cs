using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FreeMovePlayerState : PlayerState
{
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private TaskPlayerState taskPlayerState;

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
        if (playerCollisionController.NearDecontaminationInteraction != null)
        {
            if (!playerCollisionController.NearDecontaminationInteraction.IsOccupied())
            {
                playerCollisionController.NearDecontaminationInteraction.SetOccupied(true);
                SwitchState(playerStateMachine.goToDecontaminationPlayerState);
            }
        }
        else if (playerCollisionController.IsOnButtonArea)
        {
            if (playerCollisionController.NearDoorButtonController != null)
            {
                playerCollisionController.NearDoorButtonController.ToggleDoor();
            }
        }
        else if (playerCollisionController.IsOnTaskArea && playerCollisionController.NearTaskController.currentState is AvailableState)
        {
            if (!taskPlayerState.IsOnCooldown)
            {
                SwitchState(playerStateMachine.goToTaskState);
            }
        }
    }

    public override void LeaveState()
    {
        base.LeaveState();
        playerMovementController.enabled = false;
    }
}
