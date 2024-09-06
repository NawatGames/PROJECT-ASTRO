using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float gameOverFadeDuration = 1f;
    [SerializeField] private float jumpscareFadeDuration = 1f;
    [SerializeField] private GameObject alien;
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
            DecreaseHealth();
        }
    }


    public void DecreaseHealth()
    {
        health -= 1;
        healthText.text = "VIDA: " + health;
        if (health == 0)
        {
            onZeroHealth.Raise();
        }
    }

    public void StartGameOver()
    {
        StartCoroutine(FadeToBlackAndShowText());
    }


    private IEnumerator FadeToBlackAndShowText()
    {
        float elapsedTime = 0f;
        alien.SetActive(false);
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
        while (elapsedTime < jumpscareFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / jumpscareFadeDuration);
            preGameOverJumpscare.color = new Color(preGameOverJumpscare.color.r, preGameOverJumpscare.color.g, preGameOverJumpscare.color.b, alpha);
            yield return null;
        }

        // Fade out jumpscare
        elapsedTime = 0f;
        yield return new WaitForSecondsRealtime(1);
        while (elapsedTime < jumpscareFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / jumpscareFadeDuration);
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
        // Tocar som de derrota
        FindObjectOfType<AudioManager>().Play("GameOver");
        yield return new WaitForSecondsRealtime(3);

        GameOver();
    }


    public void TriggerDelayedGameOver()
    {
        alien.SetActive(false);
        blackScreen.color = new Color(0, 0, 0, 1); 
        
        float randomDelay = Random.Range(1f, 5f);
        
        StartCoroutine(WaitForJumpscareAndFade(randomDelay));
    }

    private IEnumerator WaitForJumpscareAndFade(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        jumpScareImage.color = new Color(1, 1, 1, 1);
        Debug.Log("Jumpscare ativado!");
        StartCoroutine(WaitAndFadeAfterJumpscare(2f));
    }

    private IEnumerator WaitAndFadeAfterJumpscare(float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        while (elapsedTime < jumpscareFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / jumpscareFadeDuration);
            jumpScareImage.color = new Color(jumpScareImage.color.r, jumpScareImage.color.g, jumpScareImage.color.b, alpha);
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime < jumpscareFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / jumpscareFadeDuration);
            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, alpha);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(3);

        GameOver();
    }


    private void GameOver()
    {
        Debug.Log("GameOver");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
