using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GeniusTask : TaskScript
{
    public GeniusMonitor upGeniusMonitor;
    public GeniusMonitor downGeniusMonitor;
    public GeniusMonitor leftGeniusMonitor;
    public GeniusMonitor rightGeniusMonitor;

    public SpriteRenderer signalLight;

    private readonly List<GeniusMonitor> _buttons = new List<GeniusMonitor>();
    public List<GeniusMonitor> computerSequence = new List<GeniusMonitor>();

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
        _buttons.Add(upGeniusMonitor);
        _buttons.Add(downGeniusMonitor);
        _buttons.Add(leftGeniusMonitor);
        _buttons.Add(rightGeniusMonitor);
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

    private void OnButtonPress(GeniusMonitor geniusMonitor){
        if (_computerTurn) return;
        if (computerSequence[playerTurn] == geniusMonitor)
        {
            geniusMonitor.StartCoroutine(geniusMonitor.Blink(playerTime));
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
        OnButtonPress(upGeniusMonitor);
    }

    protected override void OnDownPerformed(InputAction.CallbackContext value)
    {
        OnButtonPress(downGeniusMonitor);
    }

    protected override void OnLeftPerformed(InputAction.CallbackContext value)
    {
        OnButtonPress(leftGeniusMonitor);
    }

    protected override void OnRightPerformed(InputAction.CallbackContext value)
    {
        OnButtonPress(rightGeniusMonitor);
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
