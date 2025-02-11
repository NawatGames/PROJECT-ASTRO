using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StatusLight : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Light2D lightComponent;
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite orangeSprite;
    [SerializeField] private Color blueLightColor;
    [SerializeField] private Color orangeLightColor;
    [SerializeField] private float blinkInterval;
    private Sprite _currentSprite;
    private Color _currentColor;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite(baseSprite, baseColor, false);
    }

    public void TurnOnAlien()
    {
        SetSprite(blueSprite, blueLightColor);
    }
    public void TurnOnAstro()
    {
        SetSprite(orangeSprite, orangeLightColor);
    }
    public void TurnOff()
    {
        SetSprite(baseSprite, baseColor, false);
    }

    public IEnumerator Blink(int timeForTaskToBreak)
    {
        var elapsedTime = 0f;
        while (elapsedTime < timeForTaskToBreak)
        {
            SetSprite(baseSprite, baseColor, false);
            yield return new WaitForSeconds(blinkInterval);
            SetSprite(_currentSprite, _currentColor);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += Time.deltaTime;
        }
    }
    
    private void SetSprite(Sprite sprite, Color color, bool enableLight = true)
    {
        _spriteRenderer.sprite = sprite;
        lightComponent.enabled = enableLight;
        lightComponent.color = color;
        if (color == baseColor || sprite == baseSprite) return;
        _currentSprite = sprite;
        _currentColor = color;
    }
}
