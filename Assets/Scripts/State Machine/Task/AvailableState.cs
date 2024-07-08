public class AvailableState : ITaskState
{
        public ITaskState Do(TaskController task)
        {
            if(!task.wasStarted)
               return task.AvailableState;
            // wasStarted == true:
            task.wasStarted = false;
            return task.BeingDoneState;
        }
}