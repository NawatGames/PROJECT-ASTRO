using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuarantineHandler : MonoBehaviour
{
    public QuarantineManager manager;
    [SerializeField] float timerQuarantineDelay;
    [SerializeField] private bool isButtonPressed;
    [SerializeField] public bool canPressButton;

    [SerializeField] public bool isBeingUsed;

    public UnityEvent quarantineStarted;
    public UnityEvent quarantineEnded;
    [SerializeField] public bool isRoomQuarantined = false;

    // public GameObject room;
    public SpriteRenderer roomSprite;

    // Start is called before the first frame update
    void Start()
    {
        canPressButton = true;
        // roomQuaratined.AddListener();
    }

    // Update is called once per frame
    void Update()
    {
        if (canPressButton && isButtonPressed)
        {
            StartCoroutine(QuarantineToggle());
            canPressButton = false;
        }

        RoomColorDebug();

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
        else roomSprite.color = Color.white;
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
        StartCoroutine(QuaratineDelay());
        yield return null;
    }
    private IEnumerator QuaratineDelay()
    {
        yield return new WaitForSecondsRealtime(timerQuarantineDelay);
        canPressButton = true;
    }
}
