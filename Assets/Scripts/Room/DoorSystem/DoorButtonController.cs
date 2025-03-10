using UnityEngine;

public class DoorButtonController : MonoBehaviour
{
    [SerializeField] protected BoxCollider2D doorCollider;
    [SerializeField] protected SpriteRenderer doorSprite;
    [SerializeField] protected Animator doorAnimator;
    protected Animator buttonAnimator;
    
    public RoomQuarantineHandler _roomQuarantineHandler;
    
    void Awake()
    {
        buttonAnimator = GetComponent<Animator>();
        if (_roomQuarantineHandler == null)
        {
            _roomQuarantineHandler = GetComponentInParent<RoomQuarantineHandler>();
        }
    }
    
    private void OnEnable()
    {
        doorCollider.enabled = false;
        doorAnimator.SetBool("IsOpen", true);
        buttonAnimator.SetBool("IsPressed", false);
    }
    
    public virtual void ToggleDoor(){}
}
