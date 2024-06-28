using UnityEngine;

public class GameOverState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        
    }
    public IPlayerState Do(PlayerController player)
    {
        return player.GameOverState;
    }

    public void FixedDo(PlayerController player)
    {
        
    }

    public void Exit(PlayerController player)
    {
        
    }
}