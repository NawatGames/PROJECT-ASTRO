using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuarantineHandler : MonoBehaviour
{
    [SerializeField] float timerQuarantineDelay;
    [SerializeField] float timerQuarantineDuration;
    [SerializeField] private bool isButtonPressed;
    [SerializeField] private bool canPressButton;

    public UnityEvent roomQuaratined;
    [SerializeField] private bool isRoomQuarantined = false;

    // public GameObject room;
    public SpriteRenderer roomSprite;

    // Start is called before the first frame update
    void Start()
    {
        canPressButton = true;
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
            roomSprite.color = Color.red;
        }
        else roomSprite.color = Color.white;
    }

    private IEnumerator QuarantineDuration()
    {
        isRoomQuarantined = true;
        roomQuaratined.Invoke();
        yield return new WaitForSecondsRealtime(timerQuarantineDuration);
        StartCoroutine(QuaratineDelay());
    }
    private IEnumerator QuaratineDelay()
    {
        isRoomQuarantined = false;
        yield return new WaitForSecondsRealtime(timerQuarantineDelay);
        canPressButton = true;
    }
}
