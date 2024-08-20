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
    [SerializeField] private Collider2D lobbyCollider;
    [SerializeField] private GameEvent completedDecontaminationEvent;
    private float _timeRemaining;
    private bool _decontaminationNeeded = false;
    private bool onePlayerPressed = false;
    private bool twoPlayersPressed = false;

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
            if (twoPlayersPressed)
            {
                CompleteTask();
                completedDecontaminationEvent.Raise();
            }
        }
    }

    public void PlayerStartedDecontamination()
    {
        if(!onePlayerPressed)
        {
            onePlayerPressed = true;
        }
        else
        {
            twoPlayersPressed = true;
        }
    }

    public void PlayerEndedDecontamination()
    {
        if(twoPlayersPressed)
        {
            twoPlayersPressed = false;
        }
        else
        {
            onePlayerPressed = false;
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
        onePlayerPressed = false;
        twoPlayersPressed = false;
        countdownText.gameObject.SetActive(false);
        StopAllCoroutines();
        _timeRemaining = Random.Range(minIntervalUntilDecontamination,maxIntervalUntilDecontamination);
        StartCoroutine(CountdownToDecontamination()); 
    }

    private void GameOver()
    {
        _decontaminationNeeded = false;
        countdownText.gameObject.SetActive(false);
        Debug.Log("Game Over! A tarefa de descontaminação falhou.");
    }
}
