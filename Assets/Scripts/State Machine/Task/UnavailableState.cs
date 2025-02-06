public class UnavailableState : ITaskState
{
    public ITaskState Do(TaskController task)
    {
        if (task.needsToBeDone)
        {
            return task.AvailableState;
        }
        return task.UnavailableState;
    }
}