public class AvailableState : ITaskState
{
        public ITaskState Do(TaskController task)
        {
               return task.AvailableState; 
        }
}