using UnityEngine;

public class DoorButtonController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private SpriteRenderer doorSprite;
    private QuarantineHandler _quarantineHandler;

    private void OnEnable()
    {
        doorCollider.enabled = false;
        doorSprite.color = Color.green;
        _quarantineHandler = GetComponentInParent<QuarantineHandler>();
    }

    public void ToggleDoor()
    {
        if(!_quarantineHandler.canPressButton)
            return;
        _quarantineHandler.ToggleQuarantine();
        doorCollider.enabled = !doorCollider.enabled;
        doorSprite.color = doorCollider.enabled ? Color.red : Color.green;
    }
}
