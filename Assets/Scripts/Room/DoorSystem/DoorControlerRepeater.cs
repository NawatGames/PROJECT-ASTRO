using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class DoorControlerRepeater : MonoBehaviour
{
    
    private BoxCollider2D doorCollider;
    private SpriteRenderer doorSprite;
    
    
    private void OnEnable()
    {
       doorCollider = GetComponent<BoxCollider2D>();
       doorSprite = GetComponent<SpriteRenderer>();
       doorCollider.enabled = false;
       doorSprite.color = Color.green;
    }
    public void ToggleDoor()
    {
        doorCollider.enabled = !doorCollider.enabled;
        doorSprite.color = doorCollider.enabled ? Color.red: Color.green;
        
    }
}
