using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public bool IsOnTaskArea { get; private set; }
    public bool IsOnLobbyArea { get; private set; }
    public bool IsOnButtonArea { get; private set; }
    public TaskController NearTaskController { get; private set; }
    public DoorButtonController NearDoorButtonController { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isLobby = other.CompareTag("Lobby");

        if (isTask || isQuarantineButton || isLobby)
        {
            if (isLobby)
            {
                IsOnLobbyArea = true;
                return;
            }
            // Botão acima do player
            if (isTask)
            {
                IsOnTaskArea = true;
                NearTaskController = other.GetComponentInChildren<TaskController>();
                return;
            }
            NearDoorButtonController = other.GetComponentInParent<DoorButtonController>();
            IsOnButtonArea = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isLobby = other.CompareTag("Lobby");

        if (isTask || isQuarantineButton || isLobby)
        {
            // Botão acima do player
            if(isLobby)
            {
                IsOnLobbyArea = false;
                return;
            }
            if (isTask)
            {
                IsOnTaskArea = false;
                NearTaskController = null;
                return;
            }
            NearDoorButtonController = other.GetComponentInParent<DoorButtonController>();
            IsOnButtonArea = false;
        }
    }
}
