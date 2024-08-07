using UnityEngine;

public class DoorButtonController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private SpriteRenderer doorSprite;
    private RoomQuarantineHandler _roomQuarantineHandler;

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
    }
}
