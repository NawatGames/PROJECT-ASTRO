using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuarantineHandler : MonoBehaviour
{
    public QuarantineManager manager;
    [SerializeField] float timerQuarantineDelay;
    [SerializeField] float timerQuarantineDuration;
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
            StartCoroutine(QuarantineDuration());
            canPressButton = false;
        }
        if(isRoomQuarantined)
        {
            //Sala quarentenada
            roomSprite.color = Color.red;
        }
        else if(!canPressButton && !isRoomQuarantined)
        {
            //Sala que nao pode ser quarentenada
            roomSprite.color = Color.blue;
        }
        else roomSprite.color = Color.white;
    }

    private IEnumerator QuarantineDuration()
    {
        isRoomQuarantined = true;
        quarantineStarted.Invoke();
        yield return new WaitForSecondsRealtime(timerQuarantineDuration);
        StartCoroutine(QuaratineDelay());
    }
    private IEnumerator QuaratineDelay()
    {
        isRoomQuarantined = false;
        yield return new WaitForSecondsRealtime(timerQuarantineDelay);
        quarantineEnded.Invoke();
    }
}
