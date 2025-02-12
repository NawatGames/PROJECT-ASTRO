using System.Collections;
using System.Collections.Generic;
using Audio_System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class HarvestCropTask : TaskScript
{
    private float _modifiedProgressValue;

    [Header("AUDIO SAMPLES (HARVEST SCRIPT)")]
    [SerializeField] private GameObject buttonPressedAudio;


    [Header("TASK CONFIG")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private float progressValue;
    [SerializeField] private float decayValue;

    protected override void Awake()
    {
        base.Awake();
        taskName = "Harvest task";
    }
    protected override void RunTask()
    {
        base.RunTask();
        progressSlider.value = 0;
        timeSlider.value = 1;
        if (isAstro == isAstroSpecialist)
        {
            _modifiedProgressValue = 1.2f * progressValue;
        }
        else
        {
            _modifiedProgressValue = progressValue;
        }
        StartCoroutine(DecayProgressBar());
        StartCoroutine(DecayTimeBar());
    }
    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        buttonPressedAudio.GetComponent<AudioPlayer>().PlayAudio();
        progressSlider.value += _modifiedProgressValue;
        if (progressSlider.value == 1)
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
            progressSlider.value -= decayValue;

        }
    }

    IEnumerator DecayTimeBar()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (timeSlider.value == 0)
            {
                TaskMistakeLeave();
            }
            else
            {
                timeSlider.value -= Time.deltaTime / 5;
            }

        }
    }
}
