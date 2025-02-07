using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentDoorButtonControler : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private SpriteRenderer doorSprite;
    [SerializeField] private Animator doorAnimator;
    private Animator buttonAnimator;

    [SerializeField] private List<DoorButtonController> doorButtonControllers;
    private bool alreadyOpened = false;
    // Start is called before the first frame update
    void Awake()
    {
        buttonAnimator = this.GetComponent<Animator>();

    }
    private void OnEnable()
    {
        doorCollider.enabled = false;
        // doorSprite.color = Color.green;
        doorAnimator.SetBool("IsOpen", true);
        buttonAnimator.SetBool("IsPressed", false);
    }

    public void ToggleDoor()
    {
        if (doorButtonControllers == null || doorButtonControllers.Count == 0)
        {
            IndependentToggleDoor();
            return;
        }
        foreach (DoorButtonController doorButtonController in doorButtonControllers)
        {
            doorButtonController.ToggleDoor();
        }


    }

    public void IndependentToggleDoor()
    {
        if (alreadyOpened)
            return;
        doorCollider.enabled = !doorCollider.enabled;
        // doorSprite.color = doorCollider.enabled ? Color.red : Color.green;
        if (doorCollider.enabled)
        {
            doorAnimator.SetBool("IsOpen", false);
            buttonAnimator.SetBool("IsPressed", true);

            FindObjectOfType<AudioManager>().Play("DoorClose");
        }
        else if (!doorSprite.enabled)
        {
            doorAnimator.SetBool("IsOpen", true);
            buttonAnimator.SetBool("IsPressed", false);
            FindObjectOfType<AudioManager>().Play("DoorOpen");
        }
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
        // doorSprite.color = Color.green;
        doorAnimator.SetBool("IsOpen", true);
        buttonAnimator.SetBool("IsPressed", false);


    }

    public void setAlreadyOpenedFalse()
    {
        alreadyOpened = false;
    }

    public void CloseDoor()
    {
        doorCollider.enabled = true;
        // doorSprite.color = Color.red;
        doorAnimator.SetBool("IsOpen", false);
        buttonAnimator.SetBool("IsPressed", true);


    }
    // Update is called once per frame
    void Update()
    {

    }
}
