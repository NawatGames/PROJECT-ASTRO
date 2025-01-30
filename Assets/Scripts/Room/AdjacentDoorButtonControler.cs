using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentDoorButtonControler : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private SpriteRenderer doorSprite;
    [SerializeField] private DoorButtonController doorButtonController;
    
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        doorCollider.enabled = false;
        doorSprite.color = Color.green;
        
    }

    public void ToggleDoor()
    {
        
        doorButtonController.ToggleDoor();
        
    }

    public void IndependentToggleDoor()
    {
        
        doorCollider.enabled = !doorCollider.enabled;
        doorSprite.color = doorCollider.enabled ? Color.red: Color.green;
    }
    public void setColor(Color color)
    {
        doorSprite.color = color;
        
    }

    public void setCollider(bool collider)
    {
        doorCollider.enabled = collider;
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
