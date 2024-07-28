using UnityEngine;

public class GameOverState : IPlayerState
{
    public IPlayerState Do(PlayerController player)
    {
        return player.GameOverState;
    }
}