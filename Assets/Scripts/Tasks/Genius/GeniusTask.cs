using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GeniusTask : TaskScript
{
    public Color baseColor;
    public Color playerColor;
    public Color computerColor;
    public Color errorColor;

    public GameObject upButton;
    public GameObject downButton;
    public GameObject leftButton;
    public GameObject rightButton;

    public GameObject signalLight;

    private readonly List<GameObject> _buttons = new List<GameObject>();
    public List<GameObject> computerSequence = new List<GameObject>();

    private bool _computerTurn;

    [SerializeField] private int levels;
    [SerializeField] private int levelsSpecialist;
    private int playerTurn;

    // Adicionando as variveis de tempo
    [SerializeField] private float playerTime;
    [SerializeField] private float computerTime;
    [SerializeField] private float waitSequenceTime;

    private void Start()
    {
        _buttons.Add(upButton);
        _buttons.Add(downButton);
        _buttons.Add(leftButton);
        _buttons.Add(rightButton);
        taskName = "Genius task";
    }

    protected override void RunTask()
    {
        base.RunTask();
        playerTurn = 0;
        computerSequence.Clear();
        NextLevel();
    }

    private void NextLevel()
    {
        playerTurn = 0;
        if (isAstro == isAstroSpecialist)
        {
            if (computerSequence.Count >= levelsSpecialist) base.TaskSuccessful();
            else
            {
                computerSequence.Add(_buttons[Random.Range(0, _buttons.Count)]);
                StartCoroutine(ShowComputerSequence());
            }
        }
        else
        {
            if (computerSequence.Count >= levels) base.TaskSuccessful();
            else
            {
                computerSequence.Add(_buttons[Random.Range(0, _buttons.Count)]);
                StartCoroutine(ShowComputerSequence());
            }
        }
    }

    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        if (_computerTurn) return;
        if (computerSequence[playerTurn] == upButton)
        {
            StartCoroutine(upButton.GetComponent<Button>().Blink(playerColor, baseColor, playerTime));
            playerTurn++;
            if (playerTurn >= computerSequence.Count) NextLevel();
        }
        else
        {
            TaskMistakeLeave();
        }
    }

    protected override void OnDownPerformed(InputAction.CallbackContext value)
    {
        if (_computerTurn) return;
        if (computerSequence[playerTurn] == downButton)
        {
            StartCoroutine(downButton.GetComponent<Button>().Blink(playerColor, baseColor, playerTime));
            playerTurn++;
            if (playerTurn >= computerSequence.Count) NextLevel();
        }
        else
        {
            TaskMistakeLeave();
        }
    }

    protected override void OnLeftPerformed(InputAction.CallbackContext value)
    {
        if (_computerTurn) return;
        if (computerSequence[playerTurn] == leftButton)
        {
            StartCoroutine(leftButton.GetComponent<Button>().Blink(playerColor, baseColor, playerTime));
            playerTurn++;
            if (playerTurn >= computerSequence.Count) NextLevel();
        }
        else
        {
            TaskMistakeLeave();
        }
    }

    protected override void OnRightPerformed(InputAction.CallbackContext value)
    {
        if (_computerTurn) return;
        if (computerSequence[playerTurn] == rightButton)
        {
            StartCoroutine(rightButton.GetComponent<Button>().Blink(playerColor, baseColor, playerTime));
            playerTurn++;
            if (playerTurn >= computerSequence.Count) NextLevel();
        }
        else
        {
            TaskMistakeLeave();
        }
    }

    protected override void TaskMistakeLeave()
    {
        base.TaskMistakeLeave();
        foreach (var button in _buttons)
        {
            StartCoroutine(button.GetComponent<Button>().Blink(errorColor, baseColor, computerTime));
        }
    }

    private IEnumerator ShowComputerSequence()
    {
        _computerTurn = true;
        signalLight.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(waitSequenceTime);

        foreach (var button in computerSequence)
        {
            StartCoroutine(button.GetComponent<Button>().Blink(computerColor, baseColor, computerTime));
            yield return new WaitForSeconds(waitSequenceTime);
        }

        _computerTurn = false;
        signalLight.GetComponent<SpriteRenderer>().color = Color.green;
    }
}
