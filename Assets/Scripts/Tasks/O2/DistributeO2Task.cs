using UnityEngine;
using UnityEngine.InputSystem; 

public class DistributeO2Task : TaskScript
{
    [SerializeField] private ArrowController _arrowController;

    private bool _taskRunning = false;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && !_taskRunning)
        {
            StartMiniGame();
        }
    }

    private void StartMiniGame()
    {
        _taskRunning = true;
        _arrowController.StartRotation();
    }

    private void EndMiniGame()
    {
        _taskRunning = false;
        _arrowController.StopRotation();
    }
}
