using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RoomQuarantineHandler : MonoBehaviour
{
    public TaskController task;
    public QuarantineManager manager;
    [SerializeField] public bool canPressButton;
    [SerializeField] public bool isBeingUsed;
    private bool _isBeingUsedTwice;
    
    [SerializeField] private DoorButtonController doorButtonController;

    private UnityEvent<bool> _onIsUsingRoomChanged;
    public UnityEvent quarantineStarted;
    public UnityEvent quarantineEnded;

    [SerializeField] public bool isRoomQuarantined = false;
    private bool _isAlienInside;

    [SerializeField] private GameEvent gameOverEvent;
    public SpriteRenderer roomSprite;

    private bool isOnCooldown = false; // Cooldown estado da sala

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
            if (isBeingUsed)
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
            if (_isBeingUsedTwice)
                _isBeingUsedTwice = false;
            else
            {
                isBeingUsed = false;
            }
        }
    }

    private void RoomColorDebug()
    {
        if (_isAlienInside)
        {
            roomSprite.color = Color.black;
        }
        else if (isRoomQuarantined)
        {
            // Sala quarentenada
            roomSprite.color = Color.red;
        }
        else if (!canPressButton && !isRoomQuarantined)
        {
            // Sala que nao pode ser quarentenada
            roomSprite.color = Color.blue;
        }
        else
        {
            roomSprite.color = new Color(0.75f, 1, 1, 0.0275f);
        }
    }

    private IEnumerator QuarantineToggleRoutine()
    {
        if (!isRoomQuarantined && !isOnCooldown )
        {   
            isRoomQuarantined = true;
            quarantineStarted.Invoke();
            manager.DisableQuarantines(this); // Desabilita outras salas
            FindObjectOfType<AudioManager>().Play("DoorClose");
            StartCoroutine(QuarantineDelay());

        }
        else if (isRoomQuarantined)
        {
            FindObjectOfType<AudioManager>().Play("DoorOpen");
            if (_isAlienInside)
            {
                gameOverEvent.Raise();
            }
            isRoomQuarantined = false;
            quarantineEnded.Invoke();
            manager.EnableQuarantines(); // Abilita todas as salas
            StartCoroutine(QuarantineDelay());
        }
        
        yield return null;
    }

    private IEnumerator QuarantineDelay()
    {   
        isOnCooldown = true;
        yield return new WaitForSecondsRealtime(manager.getTimeQuarantineDelay());
        isOnCooldown = false;
        canPressButton = true;
    }

    public void ToggleQuarantine()
    {
        if (canPressButton)
        {
            StartCoroutine(QuarantineToggleRoutine());
            canPressButton = false;
        }
    }

    public IEnumerator AlienIsInsideTimer(int alienInsideSeconds)
    {
        _isAlienInside = true;
        FindObjectOfType<AudioManager>().Play("AlienInsideRoom");
        yield return new WaitForSeconds(alienInsideSeconds);
        FindObjectOfType<AudioManager>().Stop("AlienInsideRoom");
        _isAlienInside = false;
    }
}
