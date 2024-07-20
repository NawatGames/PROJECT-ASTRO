using UnityEngine;

public class DoorButtonController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private SpriteRenderer doorSprite;

    private void OnEnable()
    {
        doorCollider.enabled = false;
        doorSprite.color = Color.green;
    }

    public void ToggleDoor()
    {
        doorCollider.enabled = !doorCollider.enabled;
        doorSprite.color = doorCollider.enabled ? Color.red : Color.green;
    }
}
