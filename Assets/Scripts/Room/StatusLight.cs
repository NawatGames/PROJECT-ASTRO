using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StatusLight : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Light2D lightComponent;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color alienColor;
    [SerializeField] private Color astroColor;
    [SerializeField] private Color warningColor;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetColor(baseColor, false);
    }

    private void SetColor(Color color, bool enableLight = true)
    {
        _spriteRenderer.color = color;
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
    public void TurnOff()
    {
        SetColor(baseColor, false);
    }

    public IEnumerator Blink(Color color, int n_times, float interval)
    {
        for (int i = 0; i < n_times; i++)
        {
            SetColor(color);
            yield return new WaitForSeconds(interval);
            SetColor(baseColor, false);
            yield return new WaitForSeconds(interval);
        }
    }
}
