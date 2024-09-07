using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RoomQuarantineHandler : MonoBehaviour
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

    private bool _isAlienInside;
    [SerializeField] private GameEvent gameOverEvent;

    // public GameObject room;
    public SpriteRenderer roomSprite;
    public SpriteRenderer wallSprite;
    
    [SerializeField] [Range(0,1)] private float fadeVel;

    void Start()
    {
        canPressButton = true;
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
        Debug.Log(wallSprite.color.a);
        if (_isAlienInside)
        {
            roomSprite.color = Color.black;
        }
        else if (isRoomQuarantined)
        {
            //Sala quarentenada
            roomSprite.color = Color.red;
            if(wallSprite.color.a < 1) wallSprite.color = new Color(0,0,0,wallSprite.color.a+fadeVel);
        }
        else if (!canPressButton && !isRoomQuarantined)
        {
            //Sala que nao pode ser quarentenada
            roomSprite.color = Color.blue;
        }
        else
        {
            roomSprite.color = new Color(0.75f, 1, 1 ,0.0275f);
            if(wallSprite.color.a > 0) wallSprite.color = new Color(0,0,0,wallSprite.color.a-fadeVel);
        }
    }

    private IEnumerator QuarantineToggleRoutine()
    {
        if (!isRoomQuarantined)
        {
            isRoomQuarantined = true;
            quarantineStarted.Invoke();
        }
        else if (isRoomQuarantined)
        {
            if (_isAlienInside)
            {
                gameOverEvent.Raise();
            }
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

    public void ToggleQuarantine()
    {
        StartCoroutine(QuarantineToggleRoutine());
        canPressButton = false;
    }

    public IEnumerator AlienIsInsideTimer(int alienInsideSeconds)
    {
        _isAlienInside = true;
        yield return new WaitForSeconds(alienInsideSeconds);
        _isAlienInside = false;
    }
}
