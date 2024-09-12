using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RoomQuarantineHandler : MonoBehaviour
{
    public TaskController task;
    public QuarantineManager manager;

    [SerializeField] private float timerQuarantineDelay;
    [SerializeField] private float cooldownDuration = 5f;  // Duração do cooldown
    [SerializeField] public bool canPressButton;
    [SerializeField] public bool isBeingUsed;
    private bool _isBeingUsedTwice;

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
        else if (isOnCooldown)
        {
            // Cooldown ativo, sala verde
            roomSprite.color = Color.green;
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
        if (!isRoomQuarantined && !isOnCooldown && manager.CanActivateQuarantine(this))
        {
            isRoomQuarantined = true;
            quarantineStarted.Invoke();
            manager.DisableQuarantines(this); // Desabilita outras salas
            FindObjectOfType<AudioManager>().Play("DoorClose");

            isOnCooldown = true;  // Inicia o cooldown
            yield return new WaitForSeconds(cooldownDuration); // Espera o tempo do cooldown
            isOnCooldown = false; // Reseta o cooldown

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

            isOnCooldown = false; // Reseta o cooldown
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
