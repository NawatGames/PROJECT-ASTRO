using UnityEngine;

public class InteractionHint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer interactionHintSprite;
    [SerializeField] private PlayerCollisionController playerCollisionController;
    [SerializeField] private TaskPlayerState taskPlayerState; // Para checar cooldown de task
    
    public void CheckForInteractionHintUpdate()
    {
        if (playerCollisionController.IsOnTaskArea && playerCollisionController.NearTaskController.currentState is AvailableState && !taskPlayerState.IsOnCooldown
            || playerCollisionController.IsOnButtonArea && playerCollisionController.NearDoorButtonController._roomQuarantineHandler.canPressButton
            || playerCollisionController.IsOnDecontamination)
        {
            interactionHintSprite.enabled = true;
        }
        else
        {
            interactionHintSprite.enabled = false;
        }
        
    }

    public void TaskAvailabilityChanged(Component task, object _)
    {
        if (task == playerCollisionController.NearTaskController)
        {
            CheckForInteractionHintUpdate();
        }
    }
}
