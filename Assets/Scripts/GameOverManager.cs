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
    // [SerializeField] private GameObject jumpScareImage;
    [SerializeField] private int preGameOverDelay;
    [SerializeField] private int preJumpscareDelay;
    [SerializeField] private GameOverVideoController gameOverVideoController;

    private void Start()
    {
        blackScreen.color = new Color(0, 0, 0, 0);
        gameOverText.color = new Color(1, 1, 1, 0);
        // jumpScareImage.SetActive(false);
        preGameOverDelay = 3;
        preJumpscareDelay = 4;
    }

    public void StartGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        alien.SetActive(false);
        yield return FadeImage(blackScreen, 1f, gameOverFadeDuration);
        yield return new WaitForSeconds(preJumpscareDelay);
        gameOverVideoController.StartJumpscareVideo();
        yield return new WaitForSeconds((float)gameOverVideoController.getVideoPlayer().clip.length);
        gameOverVideoController.StopVideo();
        FindObjectOfType<AudioManager>().Play("GameOver");
        yield return new WaitForSeconds(preGameOverDelay);
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

    private void GameOver()
    {
        SceneManager.LoadScene("GameOverScreen");
    }
}
