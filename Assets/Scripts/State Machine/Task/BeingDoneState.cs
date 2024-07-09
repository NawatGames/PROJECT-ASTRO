public class BeingDoneState : ITaskState
{
        public ITaskState Do(TaskController task)
        {
                if (task.wasInterrupted)
                {
                        task.wasInterrupted = false;
                        return task.AvailableState;
                }
                // implementar: if wasFinished {}
                return task.BeingDoneState;
        }

        public void Enter(TaskController task)
        {
                
        }
}