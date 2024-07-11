using UnityEngine;

public class FreeMovingState : IPlayerState
{
    private Vector2 _velocity;
    private Vector2 _currentVelocity;
    private bool _isInInteractiveArea;
    private bool _isInteractingWithTask;

    public void Enter(PlayerController player)
    {
        _isInInteractiveArea = false;
        _isInteractingWithTask = false;
    }

    public IPlayerState Do(PlayerController player)
    {
        if (_isInteractingWithTask)
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

    public void OnTriggerEnter2D(Collider2D other, PlayerController player)
    {
        if (other.CompareTag("Task"))
        {
            _isInInteractiveArea = true;
            //Exibir tecla de interação acima do player ou do objeto
        }

        if (other.CompareTag("QuarantineButton"))
        {
            _isInInteractiveArea = true;
        }
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