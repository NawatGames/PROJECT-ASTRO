using UnityEngine;

public class GameOverStateOld : IPlayerState
{
    public IPlayerState Do(PlayerController player)
    {
        return player.GameOverStateOld;
    }
}