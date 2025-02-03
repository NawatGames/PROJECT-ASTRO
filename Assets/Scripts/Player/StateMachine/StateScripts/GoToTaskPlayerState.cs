using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GoToTaskPlayerState : PlayerState
{
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private PauseController pauseController;
    
    private Coroutine _goToTargetCoroutine;
    
    public override void EnterState()
    {
        if(!pauseController.IsFrozen())
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
        else
        {
            //Debug.Log("Impossível iniciar task durante pause");
            SwitchState(playerStateMachine.freeMoveState);
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
        playerAnimationController.SetMovementAnimParameters(Vector2.zero);
        SwitchState(playerStateMachine.freeMoveState);
    }
}
