public class WaitingState : ITaskState
{
        public ITaskState Do(TaskController task)
        {
                return task.WaitingState;
        }
}