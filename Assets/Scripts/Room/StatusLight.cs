using System.Collections;
using UnityEngine;

public class StatusLight : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Color baseColor;

    public Color GetColor()
    {
        return _spriteRenderer.color;
    }
        
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = baseColor;
    }

    public void ChangeColor(Color newColor)
    {
        _spriteRenderer.color = newColor;
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
