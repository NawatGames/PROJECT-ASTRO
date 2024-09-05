using System.Collections;
using UnityEngine;

public class WalkingTowardsTaskState : IPlayerState
{
    private Coroutine _walkCoroutine;
    private bool isWalkRoutineComplete;
    private Vector2 _velocity;
    private Vector2 _currentVelocity;
    private Vector2 _targetPos;
    private Vector2 _playerPos;
    
    public void Enter(PlayerController player)
    {
        if (!player.NearTaskController.playerPositioning) // Para tasks que não precisam de posicionamento (é null)
        {
            isWalkRoutineComplete = true; // Causa a troca para DoingTaskState
        }
        else
        {
            _walkCoroutine = player.StartCoroutine(WalkRoutine(player));
        }
    }
    
    public IPlayerState Do(PlayerController player)
    {
        // TODO - O único jeito de isso virar false é se acabar o tempo da task. Mas aqui o timer já estará pausado (?)
        if (!player.NearTaskController.needsToBeDone)
        {
            Debug.Log("(walk to) Task interrupted");
            return player.FreeMovingState;
        }
        if (player.InteractAction.WasPressedThisFrame())
        {
            Debug.Log("(walk to) Task cancelled");
            player.StopCoroutine(_walkCoroutine);
            return player.FreeMovingState;
        }
        if (player.GameIsOver)
        {
            return player.GameOverState;
        }

        if (isWalkRoutineComplete)
        {
            Debug.Log("Doing Task");
            return player.DoingTasksState;
        }
        return player.WalkingTowardsTaskState;
    }

    private IEnumerator WalkRoutine(PlayerController player)
    {
        // TODO Criar um método Walk(Vector2 direction) no PlayerController (para ser usado aqui e no freeMovingState) ?
        
        _targetPos = player.NearTaskController.playerPositioning.position;
        _playerPos = player.transform.position;
        
        _currentVelocity = Vector2.zero;
        // Trocar linha acima pela de baixo para evitar que o player pare (Tirar vectorZero do exit do freeMovingState tb)
        //_currentVelocity = Mathf.Clamp(Vector2.Dot((_targetPos - _playerPos).normalized, player.Rigidbody2D.velocity),0,Mathf.Infinity) * player.Rigidbody2D.velocity.normalized;
        
        while ((_velocity = _targetPos - _playerPos).magnitude > 0.01f)
        {
            _velocity = (_targetPos - _playerPos).normalized * player.MoveSpeed;

            _currentVelocity = Vector2.Lerp(_currentVelocity, _velocity, player.Acceleration * Time.fixedDeltaTime);
            
            player.Rigidbody2D.velocity = player.Rigidbody2D.velocity * player.DriftFactor + _currentVelocity * (1 - player.DriftFactor);
            
            yield return new WaitForFixedUpdate();
            _playerPos = player.transform.position; // Após yield para obter a próxima posição
        }
        player.Rigidbody2D.velocity = Vector2.zero;
        isWalkRoutineComplete = true;
    }
}