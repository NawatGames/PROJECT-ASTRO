using System.Collections.Generic;
using UnityEngine;

public class MainDoorButtonController : DoorButtonController
{
    
    [SerializeField] private List<AdjacentDoorButtonControler> adjacentDoorButtonControlers;
    [SerializeField] private List<DoorControlerRepeater> repeaters;
    private bool isPressed = false;
    [SerializeField] private AlertButtonLight alertButtonLight;

    public override void ToggleDoor()
    {
        if (!_roomQuarantineHandler.canPressButton)
            return;

        _roomQuarantineHandler.ToggleQuarantine();
        doorCollider.enabled = !doorCollider.enabled;
        if (doorCollider.enabled)
        {
            alertButtonLight.Quarantine();
        }
        else if(!doorCollider.enabled)
        {
            alertButtonLight.NotQuarantine();
        }
        // doorSprite.color = doorCollider.enabled ? Color.red : Color.green;
        isPressed = !isPressed;
        foreach (AdjacentDoorButtonControler adjacentDoorButtonControler in adjacentDoorButtonControlers)
        {
            adjacentDoorButtonControler.IndependentToggleDoor();
            adjacentDoorButtonControler.setAlreadyOpenedFalse();

        }

        foreach (DoorControlerRepeater repeater in repeaters)
        {
            repeater.ToggleDoor();
        }
        if (!_roomQuarantineHandler.canPressButton && !_roomQuarantineHandler.isRoomQuarantined)
        {
            OpenAllRoomDoors();
        }

        if (_roomQuarantineHandler.isRoomQuarantined)
        {

        }
    }
    public bool IsDoorOpen()
    {
        return !isPressed;
    }

    public void OpenDoor()
    {
        doorCollider.enabled = false;
        // doorSprite.color = Color.green;
        doorAnimator.SetBool("IsOpen", true);
        buttonAnimator.SetBool("IsPressed", false);


    }
    public void CloseDoor()
    {
        doorCollider.enabled = true;
        // doorSprite.color = Color.red;
        doorAnimator.SetBool("IsOpen", false);
        buttonAnimator.SetBool("IsPressed", true);


    }

    public void OpenAllRoomDoors()
    {
        doorCollider.enabled = false;
        // doorSprite.color = Color.green;
        doorAnimator.SetBool("IsOpen", true);
        buttonAnimator.SetBool("IsPressed", false);
        alertButtonLight.NotQuarantine();
    
        foreach (AdjacentDoorButtonControler adjacentDoorButtonControler in adjacentDoorButtonControlers)
        {
            adjacentDoorButtonControler.OpenDoor();

        }

    }

    public void CloseAllRoomDoors()
    {
        doorCollider.enabled = true;
        // doorSprite.color = Color.red;
        doorAnimator.SetBool("IsOpen", false);
        buttonAnimator.SetBool("IsPressed", true);
        alertButtonLight.Quarantine();

        foreach (AdjacentDoorButtonControler adjacentDoorButtonControler in adjacentDoorButtonControlers)
        {
            adjacentDoorButtonControler.CloseDoor();

        }
    }
}
