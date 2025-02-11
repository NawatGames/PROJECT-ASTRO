using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DecontaminationTask : MonoBehaviour
{
    [SerializeField] private float firstDecontaminationDelay = 160f;
    [SerializeField] private float minIntervalUntilDecontamination = 100f;
    [SerializeField] private float maxIntervalUntilDecontamination = 150f;
    [SerializeField] private float decontaminationWindow = 30f;
    [SerializeField] private float delayBeforeAndAfterScan = 2f;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameEvent completedDecontaminationEvent;
    [SerializeField] private Animator podDoorsAnimator;
    [SerializeField] private Animator scannerAnimator;
    public GameOverManager gameOverManager;
    private float _timeRemaining;
    private bool _decontaminationNeeded = false;
    private bool _onePlayerPressed = false;
    private bool _twoPlayersPressed = false;

    private void Start()
    {
        Time.timeScale = 1f;
        _timeRemaining = firstDecontaminationDelay;
        countdownText.gameObject.SetActive(false);
        StartCoroutine(CountdownToDecontamination());
    }

    // função chamada pelo GameEvent SomeoneEnteredDecontamination
    public void SomeoneEnteredDecontamination()
    {
        if(!_onePlayerPressed)
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
        if(_twoPlayersPressed)
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
        _decontaminationNeeded = true;
        _timeRemaining = decontaminationWindow;
        countdownText.gameObject.SetActive(true);
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
        _timeRemaining = Random.Range(minIntervalUntilDecontamination,maxIntervalUntilDecontamination);
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
