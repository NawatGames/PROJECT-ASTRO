using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GuitarHeroTask : TaskScript
{
    [SerializeField] private GameObject inputBar;
    [SerializeField] private int gameRounds;
    [SerializeField] private float gameSpeed;
    [SerializeField] private int pointsToWin;
    [SerializeField] private List<GameObject> targetsBuffer;
    [SerializeField] private List<GameObject> targetsActive;
    [SerializeField] private List<SpriteRenderer> inputSymbolsBuffer;
    public float _isButtonPressed;

    protected override void Awake()
    {
        base.Awake();
        foreach (GameObject target in targetsBuffer)
        {
            target.SetActive(false);
        }
    }
    protected override void RunTask()
    {
        base.RunTask();

        StartCoroutine(Delay());

    }
    protected override void TaskMistakeStay()
    {
        base.TaskMistakeStay();

    }
    private IEnumerator Delay()
    {
        for (int i = 0; i <= gameRounds; i++)
        {
            //tem quadrado na lista?
            if (targetsBuffer.Count > 0)
            {
                // Debug.Log("removendo");
                yield return new WaitForSecondsRealtime(2f);
                targetsBuffer[0].SetActive(true);
                targetsActive.Add(targetsBuffer[0]);
                targetsBuffer.Remove(targetsBuffer[0]);
            }
        }
        


    }
    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        if (targetsActive.Count > 0) // ta podendo apertar o botao
        {
            if (targetsActive[0].GetComponent<TargetBehavior>()._pressNow && inputAsset.Task.Up.WasPressedThisFrame()) // apertei no pivo
            {
                targetsActive[0].GetComponent<TargetBehavior>()._pressNow = false;
                Debug.Log("good timing");
                targetsBuffer.Add(targetsActive[0]);
                targetsActive[0].SetActive(false);
                targetsActive.Remove(targetsActive[0]);

            }
            else
            {
                targetsActive[0].GetComponent<TargetBehavior>()._pressNow = false; // apertei fora do pivo
                Debug.Log("bad timing");
                targetsBuffer.Add(targetsActive[0]);
                targetsActive[0].SetActive(false);
                targetsActive.Remove(targetsActive[0]);

            }
        }

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
