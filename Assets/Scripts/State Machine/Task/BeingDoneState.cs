public class BeingDoneState : ITaskState
{
        public ITaskState Do(TaskController task)
        {
                return task.BeingDoneState;
        }
}