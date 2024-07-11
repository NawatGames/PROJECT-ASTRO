using UnityEngine;

public interface IPlayerState
{
    void Enter(PlayerController player) {}
    IPlayerState Do(PlayerController player);
    void FixedDo(PlayerController player) {}
    void Exit(PlayerController player) {}
    void OnTriggerEnter2D(Collider2D other, PlayerController player) {}
    void OnTriggerExit2D(Collider2D other, PlayerController player) {}
}