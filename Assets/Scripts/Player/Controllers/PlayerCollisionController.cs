using System;
using System.Collections;
using System.Collections.Generic;
using Player.StateMachine;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine playerStateMachine;
    public bool IsOnTaskArea { get; private set; }
    public bool IsOnButtonArea { get; private set; }
    public bool IsOnDecontamination { get; private set; }
    public TaskController NearTaskController { get; private set; }
    public DoorButtonController NearDoorButtonController { get; private set; }
    public AdjacentDoorButtonControler AdjacentDoorButtonControler { get; private set; }
    
    public DecontaminationPod NearDecontaminationInteraction { get; private set; }

    private void Reset()
    {
        playerStateMachine = transform.root.GetComponentInChildren<PlayerStateMachine>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isTask = other.CompareTag("Task");
        bool isQuarantineButton = other.CompareTag("QuarantineButton");
        bool isDecontamination = other.CompareTag("InteractionDecontamination");

        if (isTask || isQuarantineButton || isDecontamination)
        {
            
            if (isDecontamination)
            {
                NearDecontaminationInteraction = other.GetComponent<DecontaminationPod>();
                IsOnDecontamination = true;
            }
            else if (isTask)
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
            playerStateMachine.interactionHint.CheckForInteractionHintUpdate();
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
                IsOnDecontamination = false;
            }
            else if (isTask)
            {
                // O If abaixo resolve o problema caso o collider do player passe por duas tasks ao mesmo tempo
                // (Nesse caso, o exit de uma task pode anular o nearTask, que continha a outra task)
                if (NearTaskController == other.GetComponentInChildren<TaskController>())
                {
                    IsOnTaskArea = false;
                    NearTaskController = null;
                }
            }
            else
            {
                NearDoorButtonController = null;
                AdjacentDoorButtonControler = null;
                IsOnButtonArea = false;
            }
            playerStateMachine.interactionHint.CheckForInteractionHintUpdate();
        }
    }
}
