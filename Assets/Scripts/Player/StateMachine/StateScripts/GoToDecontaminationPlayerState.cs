using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class GoToDecontaminationPlayerState : PlayerState
{
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private PauseController pauseController;
    
    private Coroutine _goToTargetCoroutine;
    
    public override void EnterState()
    {
        /*if(!pauseController.IsFrozen())
        {*/
            base.EnterState();
            
            _goToTargetCoroutine = StartCoroutine(playerMovementController.GoToTarget(
                playerCollisionController.NearDecontaminationPod.GetDecontaminationInsidePosition(),
                ()=> {
                    SwitchState(playerStateMachine.decontaminateState);
                }));
        /*}
        else
        {
            //Debug.Log("Impossível iniciar task durante pause");
            SwitchState(playerStateMachine.freeMoveState);
        }*/
    }

    public override void StateUpdate()
    {
        if (playerStateMachine.GameIsOver)
        {
            StopCoroutine(_goToTargetCoroutine);
            SwitchState(playerStateMachine.gameOverState);
        }
    }

    protected override void OnInteractHandler(InputAction.CallbackContext ctx)
    {
        Debug.Log("(walk to) Decontamination cancelled");
        StopCoroutine(_goToTargetCoroutine);
        playerAnimationController.SetMovementAnimParameters(Vector2.zero);
        playerCollisionController.NearDecontaminationPod.SetOccupied(false);
        SwitchState(playerStateMachine.freeMoveState);
    }
}