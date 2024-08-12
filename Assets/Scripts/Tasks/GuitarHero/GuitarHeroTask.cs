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
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private List<SpriteRenderer> inputSymbolsBuffer;

    protected override void Awake()
    {
        base.Awake();
        foreach (GameObject target in targets)
        {
            target.SetActive(false);
        }
    }
    protected override void RunTask()
    {
        base.RunTask();

        StartCoroutine(Delay());



    }
    private IEnumerator Delay()
    {
        for (int i = 0; i < gameRounds; i++)
        {
            //tem quadrado na lista?
            if (targets.Count > 0)
            {
                Debug.Log("removendo");
                yield return new WaitForSecondsRealtime(2f);
                targets.Remove(targets[0]);
            }
        }


    }
    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
    }

    protected override void TaskSuccessful()
    {
        base.TaskSuccessful();
        Debug.Log("Colheita bem sucedida");
    }
    public override void EndTask()
    {
        base.EndTask();
        StopAllCoroutines();
    }
}
