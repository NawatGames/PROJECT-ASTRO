using System.Collections;
using UnityEngine;

public class RedSignalController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer redSignalSprite;
    [SerializeField] private float redSignalDuration = 5f; // Tempo total de piscar (em segundos)
    [SerializeField] private float blinkInterval = 0.5f;     // Intervalo entre as trocas de alpha (em segundos)

    private void Awake()
    {
        if(redSignalSprite != null)
            SetSpriteAlpha(0);
    }

    public void StartRedSignal()
    {
        StartCoroutine(BlinkRedSignal());
    }

    private IEnumerator BlinkRedSignal()
    {
        float elapsedTime = 0f;
        bool isOn = false;
        
        while (elapsedTime < redSignalDuration)
        {
            isOn = !isOn;
            SetSpriteAlpha(isOn ? 1f : 0f);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }
        
        SetSpriteAlpha(0);
    }

    private void SetSpriteAlpha(float alpha)
    {
        if (redSignalSprite == null)
            return;
        Color color = redSignalSprite.color;
        color.a = alpha;
        redSignalSprite.color = color;
    }
}
