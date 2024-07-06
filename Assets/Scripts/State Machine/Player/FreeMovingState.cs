using UnityEngine;

public class FreeMovingState : IPlayerState
{
    private Vector2 _velocity;
    private Vector2 _currentVelocity;

    public IPlayerState Do(PlayerController player)
    {
        if (Input.GetKeyDown(player.InteractKey) && true /*Verificar se está na área de interação da task*/)
        {
            Debug.Log("Fazendo task");
            return player.DoingTasksState;
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