using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float gameOverFadeDuration = 1f;
    [SerializeField] private GameEvent onZeroHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Image blackScreen;

    private void Start()
    {
        blackScreen.color = new Color(0, 0, 0, 0);
        gameOverText.color = new Color(255, 255, 255, 0);
        healthText.text = "VIDA: " + health;
        //Coroutine exclusiva de testes
        //StartCoroutine(TestGameOver());
    }

    public void DecreaseHealth()
    {
        health -= 1;
        healthText.text = "VIDA: " + health;
        if (health == 0)
        {
            onZeroHealth.Raise();
            StartGameOver();
        }
    }
    private void StartGameOver()
    {
        StartCoroutine(FadeToBlackAndShowText());
    }

    //Método exclusivo para testes
    private IEnumerator TestGameOver()
    {
        health += 1;
        while (health != 0)
        {
            DecreaseHealth();
            yield return new WaitForSeconds(2);
        }
        yield return null;
    }
    
    private IEnumerator FadeToBlackAndShowText()
    {
        float elapsedTime = 0f;

        // Fade to black
        while (elapsedTime < gameOverFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / gameOverFadeDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        elapsedTime = 0f;

        // Fade in "GAME OVER" text
        while (elapsedTime < gameOverFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime /gameOverFadeDuration);
            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, alpha);
            yield return null;
        }
    }
}
