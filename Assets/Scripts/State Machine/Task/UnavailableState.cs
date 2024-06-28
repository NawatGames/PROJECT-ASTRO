public class UnavailableState : ITaskState
{
        public ITaskState Do(TaskController task)
        {
                if (task.NeedsToBeDone)
                {
                        return task.AvailableState;
                }
                return task.UnavailableState;
        }
}