using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Serialization;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class GuitarHeroTask : TaskScript
{
    [SerializeField] private GameObject inputBar;
    [SerializeField] private int maxBlockPoints;
    [SerializeField] private float blockSpeed;
    private float _blockSpace = 2f;

    [FormerlySerializedAs("mistakes")][SerializeField] private int totalMistakes;
    [SerializeField] private int numberOfPossibleMistakes;

    [SerializeField] private int pointsMade;
    [SerializeField] private List<GameObject> targetsBuffer;
    [SerializeField] private List<GameObject> targetsActive;
    [SerializeField] private List<SpriteRenderer> inputSymbols;
    [SerializeField] private int auxPointsToWin;
    [SerializeField] private Transform targetsStartLocation;
    protected override void Awake()
    {
        base.Awake();

        auxPointsToWin = numberOfPossibleMistakes;
        pointsMade = 0;
        totalMistakes = 0;
        foreach (GameObject target in targetsBuffer)
        {
            target.SetActive(false);
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
        if (targetsBuffer.Count > 0)
        {
            for (int i = 1; i <= maxBlockPoints; i++)
            {

                // Debug.Log("removendo");
                yield return new WaitForSecondsRealtime(_blockSpace);
                RemoveTargetInBuffer();
            }
            while (targetsActive.Count > 0)
            {
                yield return null;
            }
            if (pointsMade >= maxBlockPoints)
            {
                TaskSuccessful();

            }
            else
            {
                TaskMistakeLeave();
            }
        }
    }
    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        int symbolPressed = 0; // simbolo de seta pra cima
        if (targetsActive.Count > 0) // ta podendo apertar o botao
        {
            if (inputAsset.Task.Up.WasPressedThisFrame())
            {
                VerifyPoint(symbolPressed);
            }
        }


    }
    protected override void OnDownPerformed(InputAction.CallbackContext value)
    {
        int symbolPressed = 1; // simbolo de seta pra baixo
        if (targetsActive.Count > 0) // ta podendo apertar o botao
        {
            if (inputAsset.Task.Down.WasPressedThisFrame())
            {
                VerifyPoint(symbolPressed);
            }
        }

    }
    protected override void OnLeftPerformed(InputAction.CallbackContext value)
    {
        base.OnLeftPerformed(value);
        int symbolPressed = 2; // simbolo de seta pra esquerda
        if (targetsActive.Count > 0) // ta podendo apertar o botao
        {
            if (inputAsset.Task.Left.WasPressedThisFrame())
            {
                VerifyPoint(symbolPressed);
            }
        }

    }
    protected override void OnRightPerformed(InputAction.CallbackContext value)
    {
        base.OnLeftPerformed(value);
        int symbolPressed = 3; // simbolo de seta pra direita
        if (targetsActive.Count > 0) // ta podendo apertar o botao
        {
            if (inputAsset.Task.Right.WasPressedThisFrame())
            {
                VerifyPoint(symbolPressed);
            }

        }

    }

    private void VerifyPoint(int symbolPressed)
    {

        if (targetsActive[0].GetComponent<TargetBehavior>()._pressNow)
        {

            if (targetsActive[0].GetComponent<TargetBehavior>().symbol == symbolPressed)
            {
                Debug.Log("good timing");
                InsertTargetInBuffer();
                pointsMade++;

                if (pointsMade == maxBlockPoints)
                {

                    TaskSuccessful();
                }

            }
            else if (targetsActive[0].GetComponent<TargetBehavior>().symbol != symbolPressed)
            {
                Debug.Log("failed");
                InsertTargetInBuffer();
                totalMistakes++;
                if (totalMistakes > numberOfPossibleMistakes)
                {

                    TaskMistakeLeave();

                }
            }
        }
        else if (!targetsActive[0].GetComponent<TargetBehavior>()._pressNow)
        {
            Debug.Log("failed");
            InsertTargetInBuffer();
            totalMistakes++;
            if (totalMistakes > numberOfPossibleMistakes)
            {

                TaskMistakeLeave();

            }

        }

    }

    public void InsertTargetInBuffer()
    {
        // Debug.Log(targetsActive[0]);
        targetsActive[0].GetComponent<TargetBehavior>()._pressNow = false;
        targetsBuffer.Add(targetsActive[0]);
        targetsActive[0].SetActive(false);
        
        targetsActive[0].transform.position = targetsStartLocation.position;

        targetsActive.Remove(targetsActive[0]);
    }
    private void RemoveTargetInBuffer()
    {
        targetsBuffer[0].SetActive(true);
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
        if (totalMistakes > numberOfPossibleMistakes)
        {
            TaskMistakeLeave();
        }
    }

}