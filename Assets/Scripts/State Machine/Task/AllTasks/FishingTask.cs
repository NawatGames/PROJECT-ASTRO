using UnityEngine;
using UnityEngine.InputSystem;

public class FishingTask : TaskScript
{
    protected override void RunTask()
    {
        Debug.Log("FISHING");
    }

    protected override void OnDownPerformed(InputAction.CallbackContext value)
    {
        Debug.Log("TASK DOWN");
    }
}
