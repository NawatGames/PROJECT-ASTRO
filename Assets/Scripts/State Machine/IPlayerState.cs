public interface IPlayerState
{
    void Enter(PlayerController player);
    IPlayerState Do(PlayerController player);
    void FixedDo(PlayerController player);
    void Exit(PlayerController player);
}