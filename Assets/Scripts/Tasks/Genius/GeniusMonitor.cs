using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class GeniusMonitor : MonoBehaviour
{
    [SerializeField] private Light2D monitorLight;
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
        monitorLight.enabled = true;

        yield return new WaitForSeconds(time);

        running = false;
        _spriteRenderer.enabled = false;
        monitorLight.enabled = false;
    }
}
