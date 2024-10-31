using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private float gameOverFadeDuration = 1f;
    [SerializeField] private float jumpscareFadeDuration = 1f;
    [SerializeField] private GameObject alien;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Image blackScreen;
    [SerializeField] private Image preGameOverJumpscare;
    [SerializeField] private Image jumpScareImage;
    [SerializeField] private int preGameOverDelay;
    [SerializeField] private int preJumpscareDelay;
    [SerializeField] private int preFadeInDelay;
    [SerializeField] private int preImageDelay;

    private void Start()
    {
        blackScreen.color = new Color(0, 0, 0, 0);
        gameOverText.color = new Color(255, 255, 255, 0);
        preGameOverJumpscare.color = new Color(1, 1, 1, 0);
        jumpScareImage.gameObject.SetActive(false);
        preGameOverDelay = 3;
        preFadeInDelay = 1;
        preJumpscareDelay = 1;
        preImageDelay = 2;
    }

    public void StartGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        alien.SetActive(false);
        yield return FadeImage(blackScreen, 1f, gameOverFadeDuration); // Fade in black screen

        yield return new WaitForSeconds(preFadeInDelay);
        yield return FadeImage(preGameOverJumpscare, 1f, jumpscareFadeDuration); // Fade in pre-game over jumpscare

        yield return new WaitForSeconds(preJumpscareDelay);
        yield return FadeImage(preGameOverJumpscare, 0f, jumpscareFadeDuration); // Fade out jumpscare

        yield return FadeText(gameOverText, 1f, gameOverFadeDuration); // Fade in game over text
        FindObjectOfType<AudioManager>().Play("GameOver");

        yield return new WaitForSeconds(preGameOverDelay);
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

        jumpScareImage.gameObject.SetActive(true);
        jumpScareImage.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(2);

        yield return FadeImage(jumpScareImage, 0f, jumpscareFadeDuration);
        jumpScareImage.gameObject.SetActive(false);

        yield return FadeText(gameOverText, 1f, gameOverFadeDuration);
        FindObjectOfType<AudioManager>().Play("GameOver");
        yield return new WaitForSeconds(preImageDelay);

        GameOver();
    }

    private IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float targetAlpha, float duration)
    {
        float startAlpha = text.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOverScreen");
    }
}
