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
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class GuitarHeroTask : TaskScript
{
    [SerializeField] private GameObject inputBar;
    [SerializeField] private int maxBlockPoints;
    [SerializeField] private float blockSpeed;
    private float _blockSpace = 2f;
    [Range(0.5f, 1f)][SerializeField] private float percentToWin;
    [SerializeField] private int pointsToWin;
    [SerializeField] private int pointsMade;
    [SerializeField] private List<GameObject> targetsBuffer;
    [SerializeField] private List<GameObject> targetsActive;
    [SerializeField] private List<SpriteRenderer> inputSymbols;
    
    [SerializeField] private TMP_Text pointsToWinText;
    [SerializeField] private TMP_Text checkText;

    protected override void Awake()
    {
        base.Awake();
        pointsToWin = (int)Math.Round(maxBlockPoints * percentToWin);
        pointsToWinText.text = "";
        checkText.text = "";
        pointsMade = 0;
        foreach (GameObject target in targetsBuffer)
        {
            target.SetActive(false);
        }
    }
    protected override void RunTask()
    {
        base.RunTask();

        StartCoroutine(GameRound());
        PointDisplay();

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
            if (pointsMade >= pointsToWin)
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
            // if (targetsActive[0].GetComponent<TargetBehavior>()._pressNow &&
            //  inputAsset.Task.Up.WasPressedThisFrame() && targetsActive[0].GetComponent<TargetBehavior>().symbol == symbolPressed) // apertei no pivo
            // {
            //     Debug.Log("good timing");
            //     InsertTargetInBuffer();
            //     pointsMade++;
            // }
            // else
            // {
            //     Debug.Log("failed");
            //     InsertTargetInBuffer();
            //     pointsMade--;
            // }
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
                //Arthur
                if(pointsMade == pointsToWin)
                {
                    
                    TaskSuccessful();
                    EndTask();
                }
                
            }
            else if (targetsActive[0].GetComponent<TargetBehavior>().symbol != symbolPressed)
            {
                Debug.Log("failed");
                InsertTargetInBuffer();
                // pointsMade--;
            }
        }
        else if (!targetsActive[0].GetComponent<TargetBehavior>()._pressNow)
        {
            Debug.Log("failed");
            InsertTargetInBuffer();
            // pointsMade--;
        }
        PointDisplay();
    }

    public void InsertTargetInBuffer()
    {
        Debug.Log(targetsActive[0]);
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
        
        pointsToWinText.text = "task bem sucedida!";
        Invoke("DisapearDisplay",2);
    }
    public override void EndTask()
    {
        
        base.EndTask();
        //Por algum motivo um deles continua ativo
        targetsActive[0].SetActive(false);
        // if (pointsMade == 0)
        // {
        //     pointsToWinText.text = "task concluida com sucesso!";
        // }
        // else
        // {
        //     pointsToWinText.text = "task falhou!";
        // }
        StopAllCoroutines();
        // Invoke("DisapearDisplay",2);
    }

    public float GetGameSpeed()
    {
        return blockSpeed;
    }

    public void PointDisplay()
    {
        pointsToWinText.text = (pointsToWin-pointsMade).ToString() ;
    }
    
    public void DisapearDisplay()
    {
        pointsToWinText.text = "";
    }
    
    public void CheckDisplay()
    {   
        
        checkText.text = "Good Timing!";
        
    }
}
