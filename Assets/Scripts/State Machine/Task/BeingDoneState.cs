public class BeingDoneState : ITaskState
{
    public ITaskState Do(TaskController task)
    {
        if (task.wasInterrupted)
        {
            task.wasInterrupted = false;
            return task.AvailableState;
        }

        if (!task.needsToBeDone)
        {
            return task.UnavailableState;
        }
        return task.BeingDoneState;
    }

    public void Exit(TaskController task)
    {
        task.taskScript.EndTask();
    }
}