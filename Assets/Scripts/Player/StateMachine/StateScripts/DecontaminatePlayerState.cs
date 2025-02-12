using Player.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(GameEventListener))]
public class DecontaminatePlayerState : PlayerState
{
    private GameEventListener _gameEventListener;
    private bool _isDecontaminating;
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private GameEvent startedDecontaminationEvent;
    [SerializeField] private GameEvent stoppedDecontaminationEvent;
    [SerializeField] private GameEvent playersMovedAwayFromDecontaminationDoors;

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
            playerCollisionController.NearDecontaminationPod.SetOccupied(false);
            SwitchState(playerStateMachine.freeMoveState);
        }
    }
    
    private void OnCompleteDecontaminationHandler(Component c, object o)
    {
        playerCollisionController.NearDecontaminationPod.SetOccupied(false);
        StartCoroutine(playerMovementController.GoToTarget(
            playerCollisionController.NearDecontaminationPod.GetDecontaminationOutsidePosition(),
            ()=>
            {
                playersMovedAwayFromDecontaminationDoors.Raise();
                SwitchState(playerStateMachine.freeMoveState);
            }));
    }

    public override void LeaveState()
    {
        base.LeaveState();
        if (!_isDecontaminating) // Portanto, saiu pelo OnInteractHandler
        {
            stoppedDecontaminationEvent.Raise();
        }
        _isDecontaminating = false;
        _gameEventListener.response.RemoveListener(OnCompleteDecontaminationHandler);
    }
}
