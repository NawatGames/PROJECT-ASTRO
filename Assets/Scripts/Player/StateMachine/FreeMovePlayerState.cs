using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeMovePlayerState : PlayerState
{
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerMovementController playerMovementController;

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
        if (playerCollisionController.IsOnTaskArea && playerCollisionController.NearTaskController.currentState is AvailableState)
        {
            //Debug.Log("(walk)Started task");
            SwitchState(playerStateMachine.goToTaskState);
        }
        else if (playerCollisionController.IsOnEmptyLobbyArea)
        {
            SwitchState(playerStateMachine.decontaminateState);
        }
        else if (playerCollisionController.IsOnButtonArea)
        {
            if (playerCollisionController.NearDoorButtonController != null)
            {
                playerCollisionController.NearDoorButtonController.ToggleDoor();
            }

            // Verifica se há um botão adjacente e aciona sua função
            if (playerCollisionController.AdjacentDoorButtonControler != null)
            {
                playerCollisionController.AdjacentDoorButtonControler.ToggleDoor();
            }
        }
    }

    public override void LeaveState()
    {
        base.LeaveState();
        playerMovementController.enabled = false;
    }
}
