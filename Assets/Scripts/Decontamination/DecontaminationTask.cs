using System.Collections;
using Audio_System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DecontaminationTask : MonoBehaviour
{
    [Header("TASK CONFIG")]
    [SerializeField] private float firstDecontaminationDelay = 160f;
    [SerializeField] private float minIntervalUntilDecontamination = 100f;
    [SerializeField] private float maxIntervalUntilDecontamination = 150f;
    [SerializeField] private float decontaminationWindow = 30f;
    [SerializeField] private float delayBeforeAndAfterScan = 2f;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameEvent completedDecontaminationEvent;
    [SerializeField] private Animator podDoorsAnimator;
    [SerializeField] private Animator scannerAnimator;
    [SerializeField] private Collider2D closedCollider;
    [SerializeField] private Collider2D openedCollider;
    [SerializeField] private SpriteRenderer leftGradientMask;
    [SerializeField] private SpriteRenderer rightGradientMask;

    [SerializeField] private Image vignette;
    public GameOverManager gameOverManager;
    private float _timeRemaining;
    private bool _decontaminationNeeded = false;
    private bool _onePlayerPressed = false;
    private bool _twoPlayersPressed = false;
    [Header("AUDIO SAMPLES")]
    private AudioSource _audioSource;
    [SerializeField] private AudioPlayer audioPlayer;
    private bool onePlayerPressed = false;
    private bool twoPlayersPressed = false;
    private BoxCollider2D[] podColliders;

    private void Start()
    {
        _audioSource = audioPlayer.gameObject.GetComponent<AudioSource>();
        Time.timeScale = 1f;
        _timeRemaining = firstDecontaminationDelay;
        countdownText.gameObject.SetActive(false);
        podColliders = GetComponentsInChildren<BoxCollider2D>();
        foreach (var collider in podColliders)
        {
            collider.enabled = false;
        }
        StartCoroutine(CountdownToDecontamination());

    }

    // função chamada pelo GameEvent SomeoneEnteredDecontamination
    public void SomeoneEnteredDecontamination()
    {
        if (!_onePlayerPressed)
        {
            _onePlayerPressed = true;
        }
        else
        {
            _twoPlayersPressed = true;
            StartDecontaminationProcedure();
        }
    }

    // função chamada pelo GameEvent SomeoneLeftDecontamination
    public void SomeoneLeftDecontamination()
    {
        if (_twoPlayersPressed)
        {
            _twoPlayersPressed = false;
        }
        else
        {
            _onePlayerPressed = false;
        }
    }


    private IEnumerator CountdownToDecontamination()
    {
        while (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;

            if (_timeRemaining <= 0 && !_decontaminationNeeded)
            {
                StartDecontaminationWindow();
                yield break;
            }

            yield return null;
        }
    }

    private void StartDecontaminationWindow()
    {
        podDoorsAnimator.SetTrigger("Open");
        closedCollider.enabled = false;
        openedCollider.enabled = true;
        _decontaminationNeeded = true;
        _timeRemaining = decontaminationWindow;
        countdownText.gameObject.SetActive(true);

        StartCoroutine(VignetteAndHeartbeat());

        foreach (var collider in podColliders)
        {
            collider.enabled = true;
        }
        StartCoroutine(DecontaminationWindow());
    }

    private IEnumerator DecontaminationWindow()
    {
        while (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
            UpdateCountdownText();

            if (_timeRemaining <= 0)
            {
                GameOver();
            }

            yield return null;
        }
    }

    private IEnumerator VignetteAndHeartbeat()
    {
        audioPlayer.PlayLoop();
        float initialTime = _timeRemaining;

        while (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
            float progress = 1 - (_timeRemaining / initialTime);

            // Ajusta o alfa da vinheta
            var color = vignette.color;
            color.a = Mathf.Lerp(0f, 1f, progress);
            vignette.color = color;

            // Ajusta o volume do áudio
            _audioSource.volume = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        // Garante que os valores finais sejam 100%
        var finalColor = vignette.color;
        finalColor.a = 1f;
        vignette.color = finalColor;
        _audioSource.volume = 1f;
        audioPlayer.StopAudio();
    }

    private void UpdateCountdownText()
    {
        if (_decontaminationNeeded)
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60f);
            countdownText.text = $"Tempo restante para Descontaminação: {minutes:00}:{seconds:00}";
        }
    }

    private void StartDecontaminationProcedure()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
        _decontaminationNeeded = false;
        _onePlayerPressed = false;
        _twoPlayersPressed = false;
        countdownText.gameObject.SetActive(false);
        StopAllCoroutines(); // Para interromper DecontaminationWindow()
        StartCoroutine(WaitAndScan());
    }

    private IEnumerator WaitAndScan()
    {
        podDoorsAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(delayBeforeAndAfterScan);
        scannerAnimator.SetTrigger("StartScan");
        // Após a animação, CompleteDecontamination() será iniciada por AnimationEvent ...
    }

    public IEnumerator CompleteDecontamination()
    {

        yield return new WaitForSeconds(delayBeforeAndAfterScan);
        podDoorsAnimator.SetTrigger("Open");
        completedDecontaminationEvent.Raise();

        float fadeDuration = 2f; // Tempo que o fade-out deve durar
        float elapsedTime = 0f;

        float initialAlpha = vignette.color.a;
        float initialVolume = _audioSource.volume;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;

            // Ajusta o alfa da vinheta suavemente
            var color = vignette.color;
            color.a = Mathf.Lerp(initialAlpha, 0f, progress);
            vignette.color = color;

            // Ajusta o volume do áudio suavemente
            _audioSource.volume = Mathf.Lerp(initialVolume, 0f, progress);

            yield return null;
        }

        // Garante que os valores finais sejam 0
        var finalColor = vignette.color;
        finalColor.a = 0f;
        vignette.color = finalColor;

        _audioSource.volume = 0f;
        _audioSource.Stop();
        audioPlayer.StopAudio();

        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

        ResetDecontamination();


        // Evento acima faz os players sairem das portas e depois um GameEvent invoca ResetDecontamination() 
    }

    public void ResetDecontamination()
    {
        podDoorsAnimator.SetTrigger("Close");
        openedCollider.enabled = false;
        closedCollider.enabled = true;
        podColliders = GetComponentsInChildren<BoxCollider2D>();
        foreach (var collider in podColliders)
        {
            collider.enabled = false;
        }
        _timeRemaining = Random.Range(minIntervalUntilDecontamination, maxIntervalUntilDecontamination);
        StartCoroutine(CountdownToDecontamination());
    }

    private void GameOver()
    {
        _decontaminationNeeded = false;
        countdownText.gameObject.SetActive(false);
        Debug.Log("Game Over! A tarefa de descontaminação falhou.");
        gameOverManager.StartGameOver();
    }
}
