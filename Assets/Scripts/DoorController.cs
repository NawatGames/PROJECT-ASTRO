using UnityEngine;

public class DoorController : MonoBehaviour
{
    private BoxCollider2D _collider;
    private SpriteRenderer _sprite;

    private void OnEnable()
    {
        _collider = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void ToggleDoor()
    {
        _collider.enabled = !_collider.enabled;
        if (_collider.enabled)
        {
            _sprite.color = Color.red;
        }
        else
        {
            _sprite.color = Color.green;
        }
    }
}
