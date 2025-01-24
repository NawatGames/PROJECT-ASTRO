using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameEventListener))]
public class DecontaminatePlayerState : PlayerState
{
    private GameEventListener _gameEventListener;
    [SerializeField] private PlayerCollisionController playerCollisionController;

    protected override void Awake()
    {
        base.Awake();
        _gameEventListener = GetComponent<GameEventListener>();
    }

    public override void EnterState()
    {
        base.EnterState();
        playerStateMachine.startedDecontaminationEvent.Raise();
        _gameEventListener.response.AddListener(OnCompleteDecontaminationHandler);
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
        /*A linha abaixo só é necessária se quisermos permitir que o player entre em estado de
         descontaminação mesmo quando não é exigido / não tem timer de descontaminação*/
        playerCollisionController.NearDecontaminationInteraction.SetOccupied(false);
        
        playerStateMachine.stoppedDecontaminationEvent.Raise();
        SwitchState(playerStateMachine.freeMoveState);
    }
    
    public void OnCompleteDecontaminationHandler(Component c, object o)
    {
        playerCollisionController.NearDecontaminationInteraction.SetOccupied(false);
        playerStateMachine.stoppedDecontaminationEvent.Raise();
        SwitchState(playerStateMachine.freeMoveState);
    }

    public override void LeaveState()
    {
        base.LeaveState();
        _gameEventListener.response.RemoveListener(OnCompleteDecontaminationHandler);
    }
}
