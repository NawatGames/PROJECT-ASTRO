using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float gameOverFadeDuration = 1f;
    [SerializeField] private GameEvent onZeroHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Image blackScreen;
    [SerializeField] private Image preGameOverJumpscare; 
    [SerializeField] private Image jumpScareImage;

    private void Start()
    {
        blackScreen.color = new Color(0, 0, 0, 0);
        gameOverText.color = new Color(255, 255, 255, 0);
        preGameOverJumpscare.color = new Color(1, 1, 1, 0); 
        healthText.text = "VIDA: " + health;
    }

    // TESTAR O GAME OVER TIRAR DEPOIS
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TestGameOverB();
        }
    }


    public void DecreaseHealth()
    {
        health -= 1;
        healthText.text = "VIDA: " + health;
        if (health == 0)
        {
            //onZeroHealth.Raise();
            TriggerImmediateGameOver();
        }
    }

    public void StartGameOver()
    {
        StartCoroutine(FadeToBlackAndShowText());
    }

private IEnumerator FadeToBlackAndShowText()
    {
        float elapsedTime = 0f;

        yield return new WaitForSecondsRealtime(1);

        // Fade in black screen
        while (elapsedTime < gameOverFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / gameOverFadeDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Fade in jumpscare
        yield return new WaitForSecondsRealtime(1);
        elapsedTime = 0f;
        while (elapsedTime < gameOverFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / gameOverFadeDuration);
            preGameOverJumpscare.color = new Color(preGameOverJumpscare.color.r, preGameOverJumpscare.color.g, preGameOverJumpscare.color.b, alpha);
            yield return null;
        }

        // Fade out jumpscare
        elapsedTime = 0f;
        yield return new WaitForSecondsRealtime(1);
        while (elapsedTime < gameOverFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / gameOverFadeDuration);
            preGameOverJumpscare.color = new Color(preGameOverJumpscare.color.r, preGameOverJumpscare.color.g, preGameOverJumpscare.color.b, alpha);
            yield return null;
        }

        // Fade in game over text
        elapsedTime = 0f;
        while (elapsedTime < gameOverFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / gameOverFadeDuration);
            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, alpha);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(3);

        GameOver();
    }


    private void TriggerImmediateGameOver()
    {
        blackScreen.color = new Color(0, 0, 0, 1); 
        float randomDelay = Random.Range(1f, 5f);
        StartCoroutine(WaitForJumpscare(randomDelay));
    }

    private IEnumerator WaitForJumpscare(float delay)
    {
        yield return new WaitForSeconds(delay);
        TriggerJumpscare();
    }


    private void TriggerJumpscare()
    {
        jumpScareImage.color = new Color(1, 1, 1, 1);
        Debug.Log("Jumpscare ativado!");
        StartCoroutine(WaitAndFadeAfterJumpscare(2f));
    }

    private IEnumerator WaitAndFadeAfterJumpscare(float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        while (elapsedTime < gameOverFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / gameOverFadeDuration);
            jumpScareImage.color = new Color(jumpScareImage.color.r, jumpScareImage.color.g, jumpScareImage.color.b, alpha);
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime < gameOverFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / gameOverFadeDuration);
            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, alpha);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(3);

        GameOver();
    }


    private void TestGameOverB()
    {
        DecreaseHealth();
    }


    private void GameOver()
    {
        Debug.Log("GameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
