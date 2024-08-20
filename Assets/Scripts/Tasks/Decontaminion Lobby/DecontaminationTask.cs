using System.Collections;
using UnityEngine;
using TMPro;

public class DecontaminationTask : MonoBehaviour
{
    [SerializeField] private float timeUntilDecontamination = 120f;
    [SerializeField] private float decontaminationWindow = 30f;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Collider2D lobbyCollider;
    [SerializeField] private GameEvent completedDecontaminationEvent;
    private float _timeRemaining;
    private int playersInLobby = 0;
    private bool _decontaminationNeeded = false;
    private bool _taskCompleted = false;
    private bool onePlayerPressed = false;
    private bool twoPlayersPressed = false;

    private void Start()
    {
        Time.timeScale = 1f;
        _timeRemaining = timeUntilDecontamination;
        countdownText.gameObject.SetActive(false);
        StartCoroutine(CountdownToDecontamination());
    }

    private void Update()
    {
        if (_decontaminationNeeded && playersInLobby >= 2)
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

    private IEnumerator CountdownToDecontamination()
    {
        while (_timeRemaining > 0 && !_taskCompleted)
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
        while (_timeRemaining > 0 && !_taskCompleted)
        {
            _timeRemaining -= Time.deltaTime;
            UpdateCountdownText();

            if (_timeRemaining <= 0 && !_taskCompleted)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInLobby++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInLobby--;
        }
    }

    private void CompleteTask()
    {
        _taskCompleted = true;
        _decontaminationNeeded = false;
        countdownText.gameObject.SetActive(false);
        StopAllCoroutines();
        Debug.Log("Descontaminação Completa!");
    }

    private void GameOver()
    {
        _decontaminationNeeded = false;
        countdownText.gameObject.SetActive(false);
        Debug.Log("Game Over! A tarefa de descontaminação falhou.");
    }
}