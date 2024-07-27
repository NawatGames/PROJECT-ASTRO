using UnityEngine;

public class FreeMovingState : IPlayerState
{
    private Vector2 _velocity;
    private Vector2 _currentVelocity;

    public IPlayerState Do(PlayerController player)
    {
        if (player.InteractAction.WasPressedThisFrame())
        {
            if (player.IsOnTaskArea && player.NearTaskController.currentState is AvailableState)
            {
                //Debug.Log("Fazendo task");
                player.NearTaskController.wasStarted = true;
                return player.DoingTasksState;
            }
            if (player.IsOnButtonArea)
            {
                player.NearDoorButtonController.ToggleDoor();
            }
        }
        if (player.GameIsOver)
        {
            return player.GameOverState;
        }
        return player.FreeMovingState;
        
    }

    public void FixedDo(PlayerController player)
    {
        Move(player);
    }

    public void Exit(PlayerController player)
    {
        _currentVelocity = Vector2.zero;
        player.Rigidbody2D.velocity = Vector2.zero;
    }
    
    private void Move(PlayerController player)
    {
        _velocity = player.MoveVector * player.MoveSpeed;
        
        _currentVelocity = Vector2.Lerp(_currentVelocity, _velocity, player.Acceleration * Time.fixedDeltaTime);

        player.Rigidbody2D.velocity = player.Rigidbody2D.velocity * player.DriftFactor + _currentVelocity * (1 - player.DriftFactor);
    }
}