using Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TaskPlayerState : PlayerState
{
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private PlayerMovementController movementController;
    [SerializeField] private float cooldownTime = 2f; 
    public bool IsOnCooldown {get; private set;}= false;
    
    public override void EnterState()
    {
        base.EnterState();
        if (IsOnCooldown)
        {
            Debug.Log("Player esta no cooldown.");
            SwitchState(playerStateMachine.freeMoveState);
            return;
        }

        movementController.OnStop();
        playerCollisionController.NearTaskController.wasStarted = true;
        playerCollisionController.NearTaskController.taskScript.SetupAndRun(playerInputController, playerStateMachine.isAstro);
    }

    public override void StateUpdate()
    {
        if (!playerCollisionController.NearTaskController.needsToBeDone)
        {
            Debug.Log("Task Parou.");
            ApplyCooldown();
            SwitchState(playerStateMachine.freeMoveState);
        }
        else if (playerStateMachine.GameIsOver)
        {
            SwitchState(playerStateMachine.gameOverState);
        }
    }
    
    protected override void OnInteractHandler(InputAction.CallbackContext ctx)
    {
        Debug.Log("Player saiu da task.");
        playerCollisionController.NearTaskController.wasInterrupted = true;
        ApplyCooldown();
        SwitchState(playerStateMachine.freeMoveState);
    }

    private void ApplyCooldown()
    {
        if (!IsOnCooldown)
        {
            IsOnCooldown = true;
            StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        Debug.Log("Cooldown de " + cooldownTime + " s.");
        
        yield return new WaitForSeconds(cooldownTime);
        
        IsOnCooldown = false;
        Debug.Log("Fim do coolwdown");
    }
}
