using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameEventListener))]
public class DecontaminatePlayerState : PlayerState
{
    private GameEventListener _gameEventListener;

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
        playerStateMachine.stoppedDecontaminationEvent.Raise();
        SwitchState(playerStateMachine.freeMoveState);
    }
    
    public void OnCompleteDecontaminationHandler(Component c, object o)
    {
        playerStateMachine.stoppedDecontaminationEvent.Raise();
        SwitchState(playerStateMachine.freeMoveState);
    }

    public override void LeaveState()
    {
        base.LeaveState();
        _gameEventListener.response.RemoveListener(OnCompleteDecontaminationHandler);
    }
}
