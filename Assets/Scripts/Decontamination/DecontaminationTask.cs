using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class DecontaminationTask : MonoBehaviour
{
    [SerializeField] private float firstDecontaminationDelay = 160f;
    [SerializeField] private float minIntervalUntilDecontamination = 100f;
    [SerializeField] private float maxIntervalUntilDecontamination = 150f;
    [SerializeField] private float decontaminationWindow = 30f;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameEvent completedDecontaminationEvent;
    [SerializeField] private Animator animator;
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

    private void Update()
    {
        if (_decontaminationNeeded)
        {
            if (_twoPlayersPressed)
            {
                CompleteTask();
                completedDecontaminationEvent.Raise();
            }
        }
    }

    public void PlayerStartedDecontamination()
    {
        if(!_onePlayerPressed)
        {
            _onePlayerPressed = true;
        }
        else
        {
            _twoPlayersPressed = true;
        }
    }

    public void PlayerEndedDecontamination()
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
        animator.SetTrigger("Open");
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

    private void CompleteTask()
    {
        _decontaminationNeeded = false;
        _onePlayerPressed = false;
        _twoPlayersPressed = false;
        countdownText.gameObject.SetActive(false);
        StopAllCoroutines();
        animator.SetTrigger("Close");
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
