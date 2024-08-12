using System.Collections;
using UnityEngine;
using TMPro;

public class DecontaminationTask : MonoBehaviour
{
    [SerializeField] private float timeUntilDecontamination = 120f;
    [SerializeField] private float decontaminationWindow = 30f;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Collider2D lobbyCollider;

    private float _timeRemaining;
    private int playersInLobby = 0;
    private bool _decontaminationNeeded = false;
    private bool _taskCompleted = false;
    private bool player1Pressed = false;
    private bool player2Pressed = false;

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
            if (Input.GetKeyDown(KeyCode.E))
            {
                player1Pressed = true;
                Debug.Log("Player 1 pressionou E");
            }

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                player2Pressed = true;
                Debug.Log("Player 2 pressionou Ctrl direito");
            }

            if (player1Pressed && player2Pressed)
            {
                CompleteTask();
            }
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

    public void EndTask()
    {
        playersInLobby = 0;
        _taskCompleted = false;
        player1Pressed = false;
        player2Pressed = false;
        countdownText.gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
