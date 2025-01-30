using UnityEngine;

public class DoorButtonController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    //[SerializeField] private BoxCollider2D adjacentDoorCollider;
     [SerializeField] private SpriteRenderer doorSprite;
     //[SerializeField] private SpriteRenderer adjacentDoorSprite;
    private RoomQuarantineHandler _roomQuarantineHandler;
    [SerializeField] private AdjacentDoorButtonControler adjacentDoorButtonControler;

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
        adjacentDoorButtonControler.IndependentToggleDoor();
       
    }
    public bool IsDoorOpen()
    {
        return !doorCollider.enabled;
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
    
}
