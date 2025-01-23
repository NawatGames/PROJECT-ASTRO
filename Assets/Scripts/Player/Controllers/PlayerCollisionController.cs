using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    public bool IsOnTaskArea { get; private set; }
    public bool IsOnButtonArea { get; private set; }
    public TaskController NearTaskController { get; private set; }
    public DoorButtonController NearDoorButtonController { get; private set; }
    
    public InteractionManager NearDecontaminationInteraction { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isDecontamination = other.CompareTag("InteractionDecontamination");

        if (isTask || isQuarantineButton || isDecontamination)
        {
            if (isDecontamination)
            {
                NearDecontaminationInteraction = other.GetComponent<InteractionManager>();
            }
            else if (isTask)
            {
                IsOnTaskArea = true;
                NearTaskController = other.GetComponentInChildren<TaskController>();
            }
            else
            {
                NearDoorButtonController = other.GetComponentInParent<DoorButtonController>();
                IsOnButtonArea = true;
            }
    
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isDecontamination = other.CompareTag("InteractionDecontamination");

        if (isTask || isQuarantineButton || isDecontamination)
        {
            if(isDecontamination)
            {
                NearDecontaminationInteraction = null; 
            }
            else if (isTask)
            {
                IsOnTaskArea = false;
                NearTaskController = null;
            }
            else
            {
                NearDoorButtonController = other.GetComponentInParent<DoorButtonController>();
                IsOnButtonArea = false;
            }
        }
    }
}
