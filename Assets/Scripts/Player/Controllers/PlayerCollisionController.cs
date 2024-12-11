using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public bool IsOnTaskArea { get; private set; }
    public bool IsOnEmptyLobbyArea { get; private set; }
    public bool IsOnButtonArea { get; private set; }
    public TaskController NearTaskController { get; private set; }
    public DoorButtonController NearDoorButtonController { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isInteractionDecontamination = other.CompareTag("InteractionDecontamination");

        if (isTask || isQuarantineButton || isInteractionDecontamination)
        {
            if (isInteractionDecontamination)
            {
                IsOnEmptyLobbyArea = true;
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
        bool isInteractionDecontamination = other.CompareTag("InteractionDecontamination");

        if (isTask || isQuarantineButton || isInteractionDecontamination)
        {
            // Botão acima do player
            if(isInteractionDecontamination)
            {
                IsOnEmptyLobbyArea = false;
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
