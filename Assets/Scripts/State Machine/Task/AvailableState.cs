public class AvailableState : ITaskState
{
    private readonly GameEvent _taskBecameAvailable;
    private readonly GameEvent _taskBecameInaccessible;
    public AvailableState(GameEvent taskBecameAvailable, GameEvent taskBecameInaccessible)
    {
        _taskBecameAvailable = taskBecameAvailable;
        _taskBecameInaccessible = taskBecameInaccessible;
    }

    public void Enter(TaskController task)
    {
        _taskBecameAvailable.Raise(task);
    }

    public ITaskState Do(TaskController task)
    {
        if(!task.wasStarted)
            return task.AvailableState;
        task.wasStarted = false;
        return task.BeingDoneState;
    }

    public void Exit(TaskController task)
    {
        _taskBecameInaccessible.Raise(task);
    }
}