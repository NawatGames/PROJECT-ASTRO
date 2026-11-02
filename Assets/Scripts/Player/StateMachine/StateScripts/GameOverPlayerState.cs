using Player.StateMachine;
using UnityEngine.InputSystem;

public class GameOverPlayerState : PlayerState
{
    public override void EnterState()
    {
        base.EnterState();
        playerInputController.input.DeactivateInput(); // Bloqueia movimento e input de task durante o jumpscare
    }

    public override void StateUpdate()
    {
        
    }

    protected override void OnInteractHandler(InputAction.CallbackContext ctx) {}
}
