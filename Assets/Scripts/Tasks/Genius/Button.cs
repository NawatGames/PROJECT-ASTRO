using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Blink(Color newColor, Color oldColor)
    {
        _spriteRenderer.color = newColor;
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.color = oldColor;
    }
}
