using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public bool running = false;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Blink(float time)
    {
        running = true;
        _spriteRenderer.enabled = true;

        yield return new WaitForSeconds(time);

        running = false;
        _spriteRenderer.enabled = false;
    }
}
