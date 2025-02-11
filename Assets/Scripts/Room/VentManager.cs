using UnityEngine;

public class VentManager : MonoBehaviour
{
    private SpriteRenderer _ventSpriteRenderer;
    [SerializeField] private Sprite closedVent;
    [SerializeField] private Sprite slightlyOpenVent;
    [SerializeField] private Sprite entirelyOpenVent;
    [SerializeField] private Transform room;

    private void Awake()
    {
        _ventSpriteRenderer = GetComponent<SpriteRenderer>();
        room = transform.parent;
    }

    public void StartInvasionWarning(Component sender, object data)
    {
        if (sender == room)
            _ventSpriteRenderer.sprite = slightlyOpenVent;
    }

    public void EndInvasionWarning(Component sender, object data)
    {
        if (sender == room)
            _ventSpriteRenderer.sprite = closedVent;
    }
    
    public void StartInvasion(Component sender, object data)
    {
        if (sender == room)
            _ventSpriteRenderer.sprite = entirelyOpenVent;
    }
}
