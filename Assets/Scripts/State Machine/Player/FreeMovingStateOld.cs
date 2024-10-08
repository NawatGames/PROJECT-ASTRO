using UnityEngine;

public class FreeMovingStateOld : IPlayerState
{
    private Vector2 _velocity;
    private Vector2 _currentVelocity;

    public IPlayerState Do(PlayerController player)
    {
        if (player.InteractAction.WasPressedThisFrame())
        {
            if (player.IsOnTaskArea && player.NearTaskController.currentState is AvailableState)
            {
                //Debug.Log("(walk)Started task");
                return player.WalkingTowardsTaskStateOld;
            }
            if (player.IsOnLobbyArea)
            {
                return player.DoingDecontaminationStateOld;
            }
            if (player.IsOnButtonArea)
            {
                player.NearDoorButtonController.ToggleDoor();
            }
        }
        if (player.GameIsOver)
        {
            return player.GameOverStateOld;
        }
        return player.FreeMovingStateOld;

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