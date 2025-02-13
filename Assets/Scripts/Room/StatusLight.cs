using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StatusLight : MonoBehaviour
{
    [SerializeField] private float warningBlinkInterval;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private string _actualTrigger;
    [SerializeField] private Light2D lightComponent;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color alienColor;
    [SerializeField] private Color astroColor;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        SetColor(baseColor, false);
    }

    private void SetColor(Color color, bool enableLight = true)
    {
        // _spriteRenderer.color = color;
        lightComponent.color = color;
        lightComponent.enabled = enableLight;
    }

    public void TurnOnOrion()
    {
        _actualTrigger = "IsOrionSpecialist";
        _animator.SetTrigger(_actualTrigger);
        SetColor(alienColor);
    }
    public void TurnOnAstro()
    {
        _actualTrigger = "IsAstroSpecialist";
        _animator.SetTrigger(_actualTrigger);
        SetColor(astroColor);
    }
    public void TurnOnWarning()
    {
        // SetColor(warningColor);
        StartCoroutine(Blink(lightComponent.color, warningBlinkInterval));
    }
    public void TurnOnSuccess()
    {
        StopAllCoroutines();
        _actualTrigger = "TaskFinished";
        StartCoroutine(Blink(_actualTrigger, successColor, 1, 1f));
    }
    public void TurnOnFailed()
    {
        StopAllCoroutines();
        _actualTrigger = "TaskFailed";
        StartCoroutine(Blink(_actualTrigger, failedColor, 1, 1f));
    }
    public void TurnOff()
    {
        SetColor(baseColor);
    }

    public IEnumerator Blink(string animatorTrigger, Color color, int n_times, float interval)
    {
        for (int i = 0; i < n_times; i++)
        {
            SetColor(color);
            _animator.SetTrigger(animatorTrigger);
            yield return new WaitForSeconds(interval);
            SetColor(baseColor, false);
            _animator.SetTrigger("TaskBase");
            yield return new WaitForSeconds(interval);
        }
    }
    public IEnumerator Blink(Color color, float interval)
    {
        while (true)
        {
            SetColor(color);
            _animator.SetTrigger("TaskBase");
            yield return new WaitForSeconds(interval);
            SetColor(baseColor, false);
            _animator.SetTrigger(_actualTrigger);
            yield return new WaitForSeconds(interval);
        }
    }
}
