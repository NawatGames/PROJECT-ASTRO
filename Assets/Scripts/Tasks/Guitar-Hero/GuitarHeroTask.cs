using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GuitarHeroTask : TaskScript
{
    [SerializeField] private GameObject inputBar;
    [SerializeField] private int maxBlockPoints;
    [SerializeField] private float blockSpeed;
    private float _blockSpace = 2f;

    [SerializeField] private int totalMistakes;
    [SerializeField] private int numberOfPossibleMistakes;

    [SerializeField] private int pointsMade;
    [SerializeField] private List<TargetBehavior> targetsBuffer;
    [SerializeField] private List<TargetBehavior> targetsActive;
    [SerializeField] private int auxPointsToWin;
    [SerializeField] private Transform targetsStartLocation;

    protected override void Awake()
    {
        base.Awake();

        auxPointsToWin = numberOfPossibleMistakes;
        pointsMade = 0;
        totalMistakes = 0;
        foreach (TargetBehavior target in targetsBuffer)
        {
            target.gameObject.SetActive(false);
            target.transform.position = targetsStartLocation.position;
        }
    }

    protected override void RunTask()
    {
        base.RunTask();
        numberOfPossibleMistakes = auxPointsToWin;
        pointsMade = 0;
        totalMistakes = 0;

        StartCoroutine(GameRound());
    }

    protected override void TaskMistakeStay()
    {
        base.TaskMistakeStay();
    }

    private IEnumerator GameRound()
    {
        if (targetsBuffer.Count <= 0) yield break;
        for (int i = 1; i <= maxBlockPoints; i++)
        {
            yield return new WaitForSeconds(_blockSpace);
            RemoveTargetInBuffer();
        }
        while (targetsActive.Count > 0)
        {
            yield return null;
        }
        if (pointsMade >= maxBlockPoints)
        {
            TaskSuccessful();
            yield break;
        }
        TaskMistakeLeave();
    }

    protected override void OnUpPerformed(InputAction.CallbackContext ctx)
    {
        // Ta podendo apertar o botao
        if (ctx.started && targetsActive.Count > 0)
        {
            VerifyPoint(SymbolEnum.Up);
        }
    }

    protected override void OnDownPerformed(InputAction.CallbackContext ctx)
    {
        // Ta podendo apertar o botao
        if (ctx.started && targetsActive.Count > 0) 
        {
            VerifyPoint(SymbolEnum.Down);
        }
    }

    protected override void OnLeftPerformed(InputAction.CallbackContext ctx)
    {
        // Ta podendo apertar o botao
        if (ctx.started && targetsActive.Count > 0) 
        {
            VerifyPoint(SymbolEnum.Left);
        }
    }

    protected override void OnRightPerformed(InputAction.CallbackContext ctx)
    {
        // Ta podendo apertar o botao
        if (ctx.started && targetsActive.Count > 0)
        {
            VerifyPoint(SymbolEnum.Right);
        }
    }

    private void VerifyPoint(SymbolEnum symbolPressed)
    {
        TargetBehavior nextTarget = targetsActive[0];
        if (nextTarget._pressNow)
        {
            if (nextTarget.symbol == symbolPressed)
            {
                Debug.Log("good timing");
                InsertTargetInBuffer();
                if (isAstro == isAstroSpecialist) pointsMade += 2;
                else pointsMade++;

                if (pointsMade >= maxBlockPoints) TaskSuccessful();
            }
            else
            {
                Debug.Log("failed");
                InsertTargetInBuffer();
                totalMistakes++;
                if (totalMistakes > numberOfPossibleMistakes) TaskMistakeLeave();
            }
        }
        else if (!targetsActive[0]._pressNow)
        {
            Debug.Log("failed");
            InsertTargetInBuffer();
            totalMistakes++;
            if (totalMistakes > numberOfPossibleMistakes) TaskMistakeLeave();
        }

    }

    public void InsertTargetInBuffer()
    {
        // Debug.Log(targetsActive[0]);
        targetsActive[0]._pressNow = false;
        targetsBuffer.Add(targetsActive[0]);
        targetsActive[0].gameObject.SetActive(false);

        targetsActive[0].transform.position = targetsStartLocation.position;

        targetsActive.Remove(targetsActive[0]);
    }

    private void RemoveTargetInBuffer()
    {
        targetsBuffer[0].gameObject.SetActive(true);
        targetsActive.Add(targetsBuffer[0]);
        targetsBuffer.Remove(targetsBuffer[0]);
    }

    protected override void TaskSuccessful()
    {
        base.TaskSuccessful();
        Debug.Log("GuitarHero bem sucedida");
        EndTask();
    }

    protected override void TaskMistakeLeave()
    {
        base.TaskMistakeLeave();
        Debug.Log("GuitarHero falhou");
        EndTask();
    }

    public override void EndTask()
    {
        base.EndTask();
        StopAllCoroutines();
    }

    public float GetGameSpeed()
    {
        return blockSpeed;
    }

    public void IncrementMistake()
    {
        totalMistakes++;
        if (totalMistakes > numberOfPossibleMistakes) TaskMistakeLeave();
    }
}
