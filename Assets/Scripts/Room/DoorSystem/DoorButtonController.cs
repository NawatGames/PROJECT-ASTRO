using System.Collections.Generic;
using UnityEngine;

public class DoorButtonController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private SpriteRenderer doorSprite;
 
    private RoomQuarantineHandler _roomQuarantineHandler;
    [SerializeField] private List<AdjacentDoorButtonControler> adjacentDoorButtonControlers;
    [SerializeField] private List<DoorControlerRepeater> repeaters;
    private bool isPressed = false;

    private void OnEnable()
    {
        doorCollider.enabled = false;
        doorSprite.color = Color.green;
        _roomQuarantineHandler = GetComponentInParent<RoomQuarantineHandler>();
    }

    public void ToggleDoor()
    {
        if(!_roomQuarantineHandler.canPressButton)
            return;
        
        _roomQuarantineHandler.ToggleQuarantine();
        doorCollider.enabled = !doorCollider.enabled;
        doorSprite.color = doorCollider.enabled ? Color.red : Color.green;
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
       
    }
    public bool IsDoorOpen()
    {
        return !isPressed;
    }

    public void OpenDoor()
    {
        doorCollider.enabled = false;
        doorSprite.color = Color.green;
    }
    public void CloseDoor()
    {
        doorCollider.enabled = true;
        doorSprite.color = Color.red;
    }

    public BoxCollider2D getDoorCollider()
    {
        return doorCollider;
    }

    public SpriteRenderer getDoorSprite()
    {
        return doorSprite;
    }

    public void OpenAllRoomDoors()
    {
        doorCollider.enabled = false;
        doorSprite.color = Color.green;
        foreach (AdjacentDoorButtonControler adjacentDoorButtonControler in adjacentDoorButtonControlers)
        {
            adjacentDoorButtonControler.OpenDoor();
            
        }
        
    }
}
