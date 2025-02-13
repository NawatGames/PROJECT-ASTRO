using System.Collections;
using Audio_System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("AUDIO SAMPLES")]
    [SerializeField] private GameObject gameOverAudio;

    [Header("GAMEOVER CONFIG")]
    [SerializeField] private float gameOverFadeDuration = 1f;
    [SerializeField] private GameObject alien;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Image blackScreen;
    [SerializeField] private int preGameOverDelay;
    [SerializeField] private int preJumpscareDelay;
    [SerializeField] private GameOverVideoController gameOverVideoController;

    private void Start()
    {
        blackScreen.color = new Color(0, 0, 0, 0);
        gameOverText.color = new Color(1, 1, 1, 0);
        preGameOverDelay = 3;
        preJumpscareDelay = 4;
    }

    [ContextMenu("Forçar game over")]
    public void StartGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    [ContextMenu("Forçar game over com delay")]
    public void StartDelayedGameOver()
    {
        StartCoroutine(DelayedGameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        

        yield return FadeImage(blackScreen, 1f, gameOverFadeDuration);

        yield return new WaitForSeconds(preJumpscareDelay);

        gameOverVideoController.GetRawImage().color = new Color(1, 1, 1, 1);
        gameOverVideoController.StartJumpscareVideo();

        yield return new WaitForSeconds((float)gameOverVideoController.GetVideoPlayer().clip.length);

        gameOverVideoController.GetRawImage().color = new Color(1, 1, 1, 0);

        gameOverAudio.GetComponent<AudioPlayer>().PlayAudio();

        yield return new WaitForSeconds(preGameOverDelay);
        alien.SetActive(false);
        GameOver();
    }

    private IEnumerator DelayedGameOverSequence()
    {
        

        yield return FadeImage(blackScreen, 1f, gameOverFadeDuration);

        yield return new WaitForSeconds(preJumpscareDelay);

        var randomTime = Random.Range(2, 8);

        yield return new WaitForSeconds(randomTime);

        gameOverVideoController.GetRawImage().color = new Color(1, 1, 1, 1);
        gameOverVideoController.StartJumpscareVideo();

        yield return new WaitForSeconds((float)gameOverVideoController.GetVideoPlayer().clip.length);

        gameOverVideoController.GetRawImage().color = new Color(1, 1, 1, 0);

        gameOverAudio.GetComponent<AudioPlayer>().PlayAudio();

        yield return new WaitForSeconds(preGameOverDelay);
        alien.SetActive(false);
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
