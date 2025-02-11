using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StatusLight : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    [SerializeField] private Light2D lightComponent;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color alienColor;
    [SerializeField] private Color astroColor;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color successColor;

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

    public void TurnOnAlien()
    {
        SetColor(alienColor);
    }
    public void TurnOnAstro()
    {
        SetColor(astroColor);
    }
    public void TurnOnWarning()
    {
        SetColor(warningColor);
    }
    public void TurnOnSuccess()
    {
        StartCoroutine(Blink("TaskFinished",successColor,1,1f));
    }
    public void TurnOff()
    {
        SetColor(baseColor);
    }

    public IEnumerator Blink(string animatorTrigger,Color color, int n_times, float interval)
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
}
