using System.Collections.Generic;
using UnityEngine;

public class DoorButtonController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private SpriteRenderer doorSprite;
    [SerializeField] private Animator doorAnimator;
    private Animator buttonAnimator;


    private RoomQuarantineHandler _roomQuarantineHandler;
    [SerializeField] private List<AdjacentDoorButtonControler> adjacentDoorButtonControlers;
    [SerializeField] private List<DoorControlerRepeater> repeaters;
    private bool isPressed = false;

    void Awake()
    {
        buttonAnimator = this.GetComponent<Animator>();

    }
    private void OnEnable()
    {
        doorCollider.enabled = false;
        doorSprite.color = Color.green;
        doorAnimator.SetBool("IsOpen", true);
        buttonAnimator.SetBool("IsPressed", false);

        _roomQuarantineHandler = GetComponentInParent<RoomQuarantineHandler>();
    }

    public void ToggleDoor()
    {
        if (!_roomQuarantineHandler.canPressButton)
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
        doorSprite.color = Color.green;
        doorAnimator.SetBool("IsOpen", true);
        buttonAnimator.SetBool("IsPressed", false);


    }
    public void CloseDoor()
    {
        doorCollider.enabled = true;
        doorSprite.color = Color.red;
        doorAnimator.SetBool("IsOpen", false);
        buttonAnimator.SetBool("IsPressed", true);


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
        doorAnimator.SetBool("IsOpen", true);
        buttonAnimator.SetBool("IsPressed", false);


        foreach (AdjacentDoorButtonControler adjacentDoorButtonControler in adjacentDoorButtonControlers)
        {
            adjacentDoorButtonControler.OpenDoor();

        }

    }

    public void CloseAllRoomDoors()
    {
        doorCollider.enabled = true;
        doorSprite.color = Color.red;
        doorAnimator.SetBool("IsOpen", false);
        buttonAnimator.SetBool("IsPressed", true);


        foreach (AdjacentDoorButtonControler adjacentDoorButtonControler in adjacentDoorButtonControlers)
        {
            adjacentDoorButtonControler.CloseDoor();

        }
    }
}
