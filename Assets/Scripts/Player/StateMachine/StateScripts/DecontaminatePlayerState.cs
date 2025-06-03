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
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameEvent someoneEnteredDecontamination;
    [SerializeField] private GameEvent someoneLeftDecontamination;
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
        someoneEnteredDecontamination.Raise();
        _gameEventListener.response.AddListener(OnCompleteDecontaminationHandler);
    }

    public override void StateUpdate()
    {
        if (playerStateMachine.GameIsOver)
        {
            SwitchState(playerStateMachine.gameOverState);
        }
    }

    public void OnStartDecontaminationProcedure()
    {
        playerSprite.enabled = false;
        _isDecontaminating = true;
    }

    protected override void OnInteractHandler(InputAction.CallbackContext ctx)
    {
        if (!_isDecontaminating)
        {
            playerCollisionController.NearDecontaminationPod.SetOccupied(false);
            SwitchState(playerStateMachine.freeMoveState);
        }
    }
    
    public void OnCompleteDecontaminationHandler(Component c, object o)
    {
        playerSprite.enabled = true;
        playerCollisionController.NearDecontaminationPod.SetOccupied(false);
        StartCoroutine(playerMovementController.GoToTarget(
            playerCollisionController.NearDecontaminationPod.GetDecontaminationOutsidePosition(),
            ()=>
            {
                playersMovedAwayFromDecontaminationDoors.Raise();
                _isDecontaminating = false;
                SwitchState(playerStateMachine.freeMoveState);
            }));
    }

    public override void LeaveState()
    {
        base.LeaveState();
        if (!_isDecontaminating) // Portanto, saiu pelo OnInteractHandler
        {
            someoneLeftDecontamination.Raise();
        }
        _isDecontaminating = false;
        _gameEventListener.response.RemoveListener(OnCompleteDecontaminationHandler);
    }
}
