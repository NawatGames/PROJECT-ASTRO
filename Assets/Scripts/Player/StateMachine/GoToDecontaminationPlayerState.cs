using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class GoToDecontaminationPlayerState : PlayerState
{
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private GameObject pauseManager;
    
    private Coroutine _goToTargetCoroutine;
    
    public override void EnterState()
    {
        pauseManager = GameObject.Find("GlobalPause");
        if(!pauseManager.GetComponent<PauseController>().IsFrozen())
        {
            base.EnterState();
            
            _goToTargetCoroutine = StartCoroutine(playerMovementController.GoToTarget(
                playerCollisionController.NearDecontaminationInteraction.GetDecontaminationPosition(),
                ()=> {
                    SwitchState(playerStateMachine.decontaminateState);
                }));
        }
        else
        {
            //Debug.Log("Impossível iniciar task durante pause");
            SwitchState(playerStateMachine.freeMoveState);
        }
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
        playerCollisionController.NearDecontaminationInteraction.SetOccupied(false);
        SwitchState(playerStateMachine.freeMoveState);
    }
}