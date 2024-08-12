using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GuitarHeroTask : TaskScript
{
    [SerializeField] private GameObject inputBar;
    [SerializeField] private int gameRounds;
    [SerializeField] private float gameSpeed;
    [SerializeField] private List<GameObject> targetsBuffer;
    [SerializeField] private List<GameObject> targetsActive;
    [SerializeField] private List<TargetBehavior> targetsScript;

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
        // Debug.Log("asd");
        foreach (TargetBehavior script in targetsScript)
        {
            _isButtonPressed = value.ReadValue<float>();
        }
    }
    protected override void OnUpCancelled(InputAction.CallbackContext value)
    {
        foreach (TargetBehavior script in targetsScript)
        {
            _isButtonPressed = value.ReadValue<float>();
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
