using System.Collections;
using UnityEngine;

public class StatusLight : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Color baseColor;
    [SerializeField] private Color alienColor;
    [SerializeField] private Color astroColor;
    [SerializeField] private Color warningColor;

    public Color GetColor()
    {
        return _spriteRenderer.color;
    }
        
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = baseColor;
    }

    public void TurnOnAlien()
    {
        _spriteRenderer.color = alienColor;
    }

    public void TurnOnAstro()
    {
        _spriteRenderer.color = astroColor;
    }

    public void TurnOnWarning()
    {
        _spriteRenderer.color = warningColor;
    }

    public void TurnOff()
    {
        _spriteRenderer.color = baseColor;
    }
    
    public IEnumerator Blink(Color color, int n_times, float interval)
    {
        for (int i = 0; i < n_times; i++)
        {
            _spriteRenderer.color = color;
            yield return new WaitForSeconds(interval);
            _spriteRenderer.color = baseColor;
            yield return new WaitForSeconds(interval);
        }
    }
}
