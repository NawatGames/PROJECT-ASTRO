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
            player.Rigidbody2D.velocity = Vector2.zero;
            return player.DoingTasksState;
        }
        Move(player);
        return player.FreeMovingState;
        
    }

    private void Move(PlayerController player)
    {
        Vector2 moveInput = Vector2.zero;

        if (Input.GetKey(player.MoveUpKey))
        {
            moveInput.y += 1;
        }
        if (Input.GetKey(player.MoveDownKey))
        {
            moveInput.y -= 1;
        }
        if (Input.GetKey(player.MoveLeftKey))
        {
            moveInput.x -= 1;
        }
        if (Input.GetKey(player.MoveRightKey))
        {
            moveInput.x += 1;
        }

        moveInput.Normalize();
        _velocity = moveInput * player.MoveSpeed;
        
        _currentVelocity = Vector2.Lerp(_currentVelocity, _velocity, player.Acceleration * Time.fixedDeltaTime);

        player.Rigidbody2D.velocity = player.Rigidbody2D.velocity * player.DriftFactor + _currentVelocity * (1 - player.DriftFactor);
    }
    
}