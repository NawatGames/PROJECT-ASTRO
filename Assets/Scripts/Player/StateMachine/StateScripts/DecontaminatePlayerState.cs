using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameEventListener))]
public class DecontaminatePlayerState : PlayerState
{
    private GameEventListener _gameEventListener;
    private bool _isDecontaminating;
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private GameEvent startedDecontaminationEvent;
    [SerializeField] private GameEvent stoppedDecontaminationEvent;

    protected override void Awake()
    {
        base.Awake();
        _gameEventListener = GetComponent<GameEventListener>();
    }

    public override void EnterState()
    {
        base.EnterState();
        playerAnimationController.ForceIdleWithDirection(Vector2.down);
        startedDecontaminationEvent.Raise();
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
        if (!_isDecontaminating)
        {
            playerCollisionController.NearDecontaminationInteraction.SetOccupied(false);
            SwitchState(playerStateMachine.freeMoveState);
        }
    }
    
    private void OnCompleteDecontaminationHandler(Component c, object o)
    {
        _isDecontaminating = false;
        playerCollisionController.NearDecontaminationInteraction.SetOccupied(false);
        SwitchState(playerStateMachine.freeMoveState);
    }

    public override void LeaveState()
    {
        base.LeaveState();
        stoppedDecontaminationEvent.Raise();
        _gameEventListener.response.RemoveListener(OnCompleteDecontaminationHandler);
    }
}
