using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInGameOverScreen : MonoBehaviour
{
    [SerializeField] private RawImage fadeImage; // Referência ao Image
    [SerializeField] private float fadeDuration = 2f; // Duração do fade

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        color.a = 0f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            color.a = elapsedTime / fadeDuration;
            fadeImage.color = color;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        color.a = 1f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            color.a = 1f - (elapsedTime / fadeDuration);
            fadeImage.color = color;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }
}