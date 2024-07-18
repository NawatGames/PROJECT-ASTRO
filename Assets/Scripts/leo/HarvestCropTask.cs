using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class HarvestCropTask : TaskScript
{
    [SerializeField] private Slider slider;
    [SerializeField] private float progressValue;
    [SerializeField] private float decayValue;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void RunTask()
    {
        base.RunTask();
        slider.value = 0;
        StartCoroutine(DecayProgressBar());
    }
    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        slider.value += progressValue;
        if (slider.value == 1)
        {
            TaskSuccessful();
        }
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

    IEnumerator DecayProgressBar()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            slider.value -= decayValue;

        }
    }

}
