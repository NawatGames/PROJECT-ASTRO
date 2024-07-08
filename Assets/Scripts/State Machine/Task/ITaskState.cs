public interface ITaskState
{
    void Enter(TaskController task) {}
    ITaskState Do(TaskController task);
    void FixedDo(TaskController task) {}
    void Exit(TaskController task) {}
}