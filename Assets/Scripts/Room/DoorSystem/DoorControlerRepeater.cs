using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class DoorControlerRepeater : MonoBehaviour
{

    private BoxCollider2D doorCollider;
    private SpriteRenderer doorSprite;
    private Animator doorAnimator;



    private void OnEnable()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        doorSprite = GetComponent<SpriteRenderer>();
        doorAnimator = GetComponent<Animator>();

        doorCollider.enabled = false;
        doorSprite.color = Color.green;
        doorAnimator.SetBool("IsOpen", true);

    }
    public void ToggleDoor()
    {
        doorCollider.enabled = !doorCollider.enabled;
        if (doorCollider.enabled)
        {
            doorAnimator.SetBool("IsOpen", false);
            doorSprite.color = Color.red;
        }
        else
        {
            doorAnimator.SetBool("IsOpen", true);
            doorSprite.color = Color.green;
        }

    }
}
