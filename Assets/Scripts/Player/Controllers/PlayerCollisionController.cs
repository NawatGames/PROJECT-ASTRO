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
    public AdjacentDoorButtonControler AdjacentDoorButtonControler { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isRight = other.CompareTag("InteractionRight");
        bool isLeft = other.CompareTag("InteractionLeft");

        if (isTask || isQuarantineButton || isRight || isLeft)
        {
            if (isRight || isLeft)
            {
                InteractionManager interactionManager = other.GetComponent<InteractionManager>();
                if(!interactionManager.IsOccupied())
                {
                    IsOnEmptyLobbyArea = true;
                    interactionManager.SetOccupied(true);
                    return;
                }

                else
                {
                    Debug.Log("Área de interação ocupada!");
                    return;
                }
            }
            if (isTask)
            {
                IsOnTaskArea = true;
                NearTaskController = other.GetComponentInChildren<TaskController>();
                
            }
            else
            {
                NearDoorButtonController = other.GetComponentInParent<DoorButtonController>();
                AdjacentDoorButtonControler = other.GetComponentInParent<AdjacentDoorButtonControler>();
                IsOnButtonArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isRight = other.CompareTag("InteractionRight");
        bool isLeft = other.CompareTag("InteractionLeft");

        if (isTask || isQuarantineButton || isRight || isLeft)
        {
            if(isRight || isLeft)
            {
                InteractionManager interactionManager = other.GetComponent<InteractionManager>();
                if(interactionManager.IsOccupied())
                {
                    interactionManager.SetOccupied(false);
                    IsOnEmptyLobbyArea = false;
                    return;
                }
            }
            if (isTask)
            {
                IsOnTaskArea = false;
                NearTaskController = null;
                
            }
            else
            {
                NearDoorButtonController = other.GetComponentInParent<DoorButtonController>();
                AdjacentDoorButtonControler= other.GetComponentInParent<AdjacentDoorButtonControler>();
                IsOnButtonArea = false;
            }
            
        }
    }
}
