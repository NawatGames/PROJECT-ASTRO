using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoToTaskPlayerState : PlayerState
{
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerMovementController playerMovementController;
    
    private Coroutine _goToTargetCoroutine;
    
    public override void EnterState()
    {
        base.EnterState();
        if (!playerCollisionController.NearTaskController.playerPositioning) // Para tasks que não precisam de posicionamento (é null)
        {
            SwitchState(playerStateMachine.taskState);
        }
        else
        {
            _goToTargetCoroutine = StartCoroutine(playerMovementController.GoToTarget(
                playerCollisionController.NearTaskController.playerPositioning.position,
                ()=> {
                    SwitchState(playerStateMachine.taskState);
                }));
        }
    }

    public override void StateUpdate()
    {
        // TODO (PAUSE TASK TIMER)- O único jeito de isso virar false é se acabar o tempo da task. Mas aqui o timer já estará pausado (?)
        if (!playerCollisionController.NearTaskController.needsToBeDone)
        {
            StopCoroutine(_goToTargetCoroutine);
            SwitchState(playerStateMachine.freeMoveState);
        }
        else if (playerStateMachine.GameIsOver)
        {
            StopCoroutine(_goToTargetCoroutine);
            SwitchState(playerStateMachine.gameOverState);
        }
    }

    protected override void OnInteractHandler(InputAction.CallbackContext ctx)
    {
        Debug.Log("(walk to) Task cancelled");
        StopCoroutine(_goToTargetCoroutine);
        SwitchState(playerStateMachine.freeMoveState);
    }
}
