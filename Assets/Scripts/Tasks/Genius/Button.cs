using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Button : MonoBehaviour
{
    [SerializeField] private Light2D light;
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
        light.enabled = true;

        yield return new WaitForSeconds(time);

        running = false;
        _spriteRenderer.enabled = false;
        light.enabled = false;
    }
}
