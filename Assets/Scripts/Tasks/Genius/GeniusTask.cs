using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GeniusTask : TaskScript
{
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    public SpriteRenderer signalLight;

    private readonly List<Button> _buttons = new List<Button>();
    public List<Button> computerSequence = new List<Button>();

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

    private void OnButtonPress(Button button){
        if (_computerTurn) return;
        if (computerSequence[playerTurn] == button)
        {
            button.StartCoroutine(button.Blink(playerTime));
            playerTurn++;
            if (playerTurn >= computerSequence.Count) NextLevel();
        }
        else
        {
            TaskMistakeLeave();
        }
    }

    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        OnButtonPress(upButton);
    }

    protected override void OnDownPerformed(InputAction.CallbackContext value)
    {
        OnButtonPress(downButton);
    }

    protected override void OnLeftPerformed(InputAction.CallbackContext value)
    {
        OnButtonPress(leftButton);
    }

    protected override void OnRightPerformed(InputAction.CallbackContext value)
    {
        OnButtonPress(rightButton);
    }

    protected override void TaskMistakeLeave()
    {
        base.TaskMistakeLeave();
    }

    private IEnumerator ShowComputerSequence()
    {
        _computerTurn = true;
        //signalLight.color = Color.red;
        
        // Espera todas as animações de blink acabarem
        foreach (var button in computerSequence){
          while(button.running) yield return null;
        }
        
        yield return new WaitForSeconds(waitSequenceTime);

        foreach (var button in computerSequence)
        {
            yield return button.StartCoroutine(button.Blink(computerTime));
            yield return new WaitForSeconds(waitSequenceTime);
        }

        _computerTurn = false;
        //signalLight.color = Color.green;
    }
}
