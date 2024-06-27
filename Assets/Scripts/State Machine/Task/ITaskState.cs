public interface ITaskState
{
    void Enter(TaskController player) {}
    ITaskState Do(TaskController player);
    void FixedDo(TaskController player) {}
    void Exit(TaskController player) {}
}