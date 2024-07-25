using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class QuarantineHandler : MonoBehaviour
{
    public TaskController task;
    
    public QuarantineManager manager;
    [SerializeField] private float timerQuarantineDelay;
    [SerializeField] public bool canPressButton;

    [SerializeField] public bool isBeingUsed;
    private bool _isBeingUsedTwice;

    private UnityEvent<bool> _onIsUsingRoomChanged;
    public UnityEvent quarantineStarted;
    public UnityEvent quarantineEnded;
    [SerializeField] public bool isRoomQuarantined = false;

    // public GameObject room;
    public SpriteRenderer roomSprite;

    void Start()
    {
        
        canPressButton = true;
        // roomQuarantined.AddListener();
    }
    
    void Update()
    {
        RoomColorDebug();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(isBeingUsed)
                _isBeingUsedTwice = true;
            else
            {
                isBeingUsed = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(_isBeingUsedTwice)
                _isBeingUsedTwice = false;
            else
            {
                isBeingUsed = false;
            }
        }
    }

    private void RoomColorDebug()
    {
        if (isRoomQuarantined)
        {
            //Sala quarentenada
            roomSprite.color = Color.red;
        }
        else if (!canPressButton && !isRoomQuarantined)
        {
            //Sala que nao pode ser quarentenada
            roomSprite.color = Color.blue;
        }
        else roomSprite.color = new Color(0.75f, 1, 1 ,0.0275f);
    }

    private IEnumerator QuarantineToggle()
    {
        if (!isRoomQuarantined)
        {
            isRoomQuarantined = true;
            quarantineStarted.Invoke();
        }
        else if (isRoomQuarantined)
        {
            isRoomQuarantined = false;
            quarantineEnded.Invoke();
        }
        StartCoroutine(QuarantineDelay());
        yield return null;
    }
    private IEnumerator QuarantineDelay()
    {
        yield return new WaitForSecondsRealtime(timerQuarantineDelay);
        canPressButton = true;
    }

    private void OnQuarantineStarted()
    {
        if (canPressButton)
        {
            StartCoroutine(QuarantineToggle());
            canPressButton = false;
        }
    }
}
