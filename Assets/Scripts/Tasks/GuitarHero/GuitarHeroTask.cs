using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GuitarHeroTask : TaskScript
{
    [SerializeField] private GameObject inputBar;
    [SerializeField] private int gameRounds;
    [SerializeField] private float gameSpeed;
    [Range(0.5f, 1f)][SerializeField] private float percentToWin;
    private int pointsToWin;
    private int pointsMade;
    [SerializeField] private List<GameObject> targetsBuffer;
    [SerializeField] private List<GameObject> targetsActive;
    [SerializeField] private List<SpriteRenderer> inputSymbols;

    protected override void Awake()
    {
        base.Awake();
        pointsToWin = (int)Math.Round(gameRounds * percentToWin);
        pointsMade = 0;
        foreach (GameObject target in targetsBuffer)
        {
            target.SetActive(false);
        }
    }
    protected override void RunTask()
    {
        base.RunTask();
        StartCoroutine(Delay());
        // StartCoroutine(VerifyCollision());
        if (pointsToWin == 0)
        {
            TaskSuccessful();
        }

    }

    // private IEnumerator VerifyCollision()
    // {
    //     if (targetsActive.Count > 0)
    //     {
    //         foreach (GameObject target in targetsActive)
    //         {
    //             if (target.GetComponent<TargetBehavior>()._passedBeyondTrigger)
    //             {
    //                 InsertTargetInBuffer();
    //                 yield return null;
    //             }
    //         }
    //     }
    // }

    protected override void TaskMistakeStay()
    {
        base.TaskMistakeStay();

    }
    private IEnumerator Delay()
    {
        if (targetsBuffer.Count > 0)
        {
            for (int i = 0; i <= gameRounds; i++)
            {

                // Debug.Log("removendo");
                yield return new WaitForSecondsRealtime(2f);
                RemoveTargetInBuffer();
            }
        }
    }
    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        int symbolPressed = 0; // simbolo de seta pra cima
        if (targetsActive.Count > 0) // ta podendo apertar o botao
        {
            if (targetsActive[0].GetComponent<TargetBehavior>()._pressNow && inputAsset.Task.Up.WasPressedThisFrame() && targetsActive[0].GetComponent<TargetBehavior>().symbol == symbolPressed) // apertei no pivo
            {
                Debug.Log("good timing");
                InsertTargetInBuffer();
                pointsMade++;
            }
            else
            {
                Debug.Log("failed");
                InsertTargetInBuffer();
                pointsMade--;
            }
        }

    }
    protected override void OnDownPerformed(InputAction.CallbackContext value)
    {
        int symbolPressed = 1; // simbolo de seta pra baixo
        if (targetsActive.Count > 0) // ta podendo apertar o botao
        {
            if (targetsActive[0].GetComponent<TargetBehavior>()._pressNow && inputAsset.Task.Up.WasPressedThisFrame() && targetsActive[0].GetComponent<TargetBehavior>().symbol == symbolPressed) // apertei no pivo
            {
                Debug.Log("good timing");
                InsertTargetInBuffer();
                pointsMade++;
            }
            else
            {
                Debug.Log("failed");
                InsertTargetInBuffer();
                pointsMade--;
            }
        }

    }
    protected override void OnLeftPerformed(InputAction.CallbackContext value)
    {
        base.OnLeftPerformed(value);
        int symbolPressed = 2; // simbolo de seta pra esquerda
        if (targetsActive.Count > 0) // ta podendo apertar o botao
        {
            if (targetsActive[0].GetComponent<TargetBehavior>()._pressNow && inputAsset.Task.Up.WasPressedThisFrame() && targetsActive[0].GetComponent<TargetBehavior>().symbol == symbolPressed) // apertei no pivo
            {
                Debug.Log("good timing");
                InsertTargetInBuffer();
                pointsMade++;
            }
            else
            {
                Debug.Log("failed");
                InsertTargetInBuffer();
                pointsMade--;
            }
        }
    }
    protected override void OnRightPerformed(InputAction.CallbackContext value)
    {
        base.OnLeftPerformed(value);
        int symbolPressed = 3; // simbolo de seta pra esquerda
        if (targetsActive.Count > 0) // ta podendo apertar o botao
        {
            if (targetsActive[0].GetComponent<TargetBehavior>()._pressNow && inputAsset.Task.Up.WasPressedThisFrame() && targetsActive[0].GetComponent<TargetBehavior>().symbol == symbolPressed) // apertei no pivo
            {
                Debug.Log("good timing");
                InsertTargetInBuffer();
                pointsMade++;
            }
            else
            {
                Debug.Log("failed");
                InsertTargetInBuffer();
                pointsMade--;
            }
        }
    }
    public void InsertTargetInBuffer()
    {
        targetsActive[0].GetComponent<TargetBehavior>()._pressNow = false;
        targetsBuffer.Add(targetsActive[0]);
        targetsActive[0].SetActive(false);
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
    }
    public override void EndTask()
    {
        base.EndTask();
        StopAllCoroutines();
    }

    public float GetGameSpeed()
    {
        return gameSpeed;
    }
}
