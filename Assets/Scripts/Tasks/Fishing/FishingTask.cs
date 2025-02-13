using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FishingTask : TaskScript
{
    #region Declaracao de variaveis
    [Header("Esperando peixe")]
    [SerializeField] private SpriteRenderer gotFishWarningSprite;
    [SerializeField] private float minFishWaitTime = 5;
    [SerializeField] private float maxFishWaitTime = 14;
    [SerializeField] private float timeToReact = 1;
    private bool _waitingForCast = false;
    private bool _waitingForFish = false;
    private bool _pressNow = false;

    [Header("Mini game")]
    [SerializeField] GameObject UI;
    [SerializeField] private TextMeshPro timerText;
    [SerializeField] private Transform controlledBar, fishIcon, progressBar;

    [Tooltip("Se esse tempo acabar, reseta a task (falhou)")]
    [SerializeField] private int timeToCatchFish = 15;

    [Tooltip("Tempo que o peixe demora para (talvez) mudar direção/velocidade")]
    [SerializeField] private float minFishDecisionTime, maxFishDecisionTime;

    [Tooltip("Modifica a chance do peixe ficar lento")]
    [SerializeField] private int fishSlowDecisionMultiplier = 2;

    [SerializeField] private float fishMinDashingVelocity = 2.5f;
    [SerializeField] private float fishMaxDashingVelocity = 5;

    [Tooltip("Maior velocidade que o peixe pode atingir no \"modo lento\"")]
    [SerializeField] private float fishMaxAbsSlowVelocity = 1;

    [SerializeField] private float maxControlledBarVelocity = 5f;
    [SerializeField] private float controlledBarAcceleration = 40f;

    [Tooltip("Multiplica o tamanho da barra controlada caso o player correto esteja fazendo a task")]
    [SerializeField] private float barSizeModifier = 1.5f;

    [Tooltip("Quanto o progresso aumenta por segundo (completando ao atingir 1)")]
    [SerializeField] private float progressSpeed = 0.125f;

    private SpriteRenderer _controlledBarSprite, _fishIconSprite, _miniGameAreaSprite;
    private float _miniGameTopBound;
    private float _miniGameBottomBound;
    private float _fishHalfHeight;
    private float _barHalfHeight;
    private float _originalControlledBarSize;
    private float _progressBarFullHeight;

    private float _currentFishVelocity = 0;
    private float _currentBarVelocity = 0;
    private float _currentProgress = 0; // de 0 a 1

    private Coroutine _currentCoroutine;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        UI.SetActive(false);
        _controlledBarSprite = controlledBar.GetComponent<SpriteRenderer>();
        _fishIconSprite = fishIcon.GetComponent<SpriteRenderer>();
        _miniGameAreaSprite = controlledBar.parent.GetComponent<SpriteRenderer>();
        _originalControlledBarSize = controlledBar.localScale.x;
        _fishHalfHeight = _fishIconSprite.bounds.extents.x;
        var gameHeight = _miniGameAreaSprite.bounds.size.x;
        _miniGameTopBound = _miniGameAreaSprite.transform.position.x + gameHeight / 2;
        _miniGameBottomBound = _miniGameTopBound - gameHeight;
        _progressBarFullHeight = progressBar.localScale.x;
        taskName = "Fishing task";
    }

    protected override void RunTask()
    {
        base.RunTask();
        UI.SetActive(true);
        Vector3 auxVector = controlledBar.localScale;
        if (isAstro != isAstroSpecialist)
        {
            auxVector.x = _originalControlledBarSize * 1;
        }
        else
        {
            auxVector.x = _originalControlledBarSize * barSizeModifier;
        }
        controlledBar.localScale = auxVector;
        _barHalfHeight = _controlledBarSprite.bounds.extents.y;

        _waitingForFish = false;
        _waitingForCast = false;
        _pressNow = false;
        _currentProgress = 0;
        auxVector = controlledBar.localPosition;
        auxVector.x = 0;
        controlledBar.localPosition = auxVector;
        auxVector = fishIcon.localPosition;
        auxVector.x = 0;
        fishIcon.localPosition = auxVector;

        // <ANIM> Iniciar animação de posicionando para pesca
        _waitingForCast = true; // Realizar apenas após animacao (animationEvent) ?
        StartCoroutine(FishChallenge());

    }

    #region SimpleFishing (Code Region)
    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        // if (_waitingForCast)
        // {
        //     _waitingForCast = false;
        //     // <ANIM> Rodar animação de lançar isca
        //     Debug.Log("Lançou isca");
        //     _currentCoroutine = StartCoroutine(WaitForFish());
        // }
        // else if (_pressNow)
        // {
        //     _pressNow = false;
        //     StopCoroutine(_currentCoroutine);
        //     Debug.Log("Good timing");
        //     _waitingForFish = false;
        //     gotFishWarningSprite.enabled = false;
        //     // activate fishChallenge sprites
        //     StartCoroutine(FishChallenge());
        // }
        // else if (_waitingForFish)
        // {
        //     WrongTiming();
        // }
    }

    private IEnumerator WaitForFish()
    {
        while (true)
        {
            _waitingForFish = true;
            yield return new WaitForSeconds(Random.Range(minFishWaitTime, maxFishWaitTime));
            _pressNow = true;
            gotFishWarningSprite.enabled = true;
            yield return new WaitForSeconds(timeToReact);
            gotFishWarningSprite.enabled = false;
            _pressNow = false;
        }
    }

    private void WrongTiming()
    {
        TaskMistakeStay();
        _waitingForFish = false;
        // <ANIM> Rodar animação de retornar isca
        StopCoroutine(_currentCoroutine);
        _waitingForCast = true;
    }
    #endregion

    #region FishChallenge (Code Region)
    private IEnumerator FishChallenge()
    {
        SetProgressBarSize();
        StartCoroutine(MovementManager());
        StartCoroutine(DecreaseFishTimer());
        while (true)
        {
            MoveFishAndBar();
            if (inputController.inputAsset.Task.Right.IsPressed())
            {
                _currentBarVelocity = Mathf.Clamp(_currentBarVelocity + controlledBarAcceleration * Time.deltaTime, -maxControlledBarVelocity, maxControlledBarVelocity);
            }
            else
            {
                _currentBarVelocity = Mathf.Clamp(_currentBarVelocity - controlledBarAcceleration * Time.deltaTime, -maxControlledBarVelocity, maxControlledBarVelocity);
            }
            CheckForProgress();
            yield return null;
        }
    }

    private IEnumerator DecreaseFishTimer()
    {
        for (int timer = timeToCatchFish; timer > 0; timer--)
        {
            timerText.text = "" + timer;
            yield return new WaitForSeconds(1);
        }
        TaskMistakeLeave();
    }

    private IEnumerator MovementManager()
    {
        int direction = Random.Range(0, 2) == 1 ? 1 : -1;
        while (true)
        {
            _currentFishVelocity = GetNewVelocity();
            yield return new WaitForSeconds(Random.Range(minFishDecisionTime, maxFishDecisionTime));
        }
    }

    private float GetNewVelocity()
    {
        switch (Random.Range(0, 2 + fishSlowDecisionMultiplier))
        {
            case 0: // Dash +
                return Random.Range(fishMinDashingVelocity, fishMaxDashingVelocity);
            case 1: // Dash -
                return -Random.Range(fishMinDashingVelocity, fishMaxDashingVelocity);
            default: // Slow down
                return Random.Range(-fishMaxAbsSlowVelocity, fishMaxAbsSlowVelocity); ;
        }
    }

    private void MoveFishAndBar()
    {
        Vector3 auxPos = fishIcon.position;
        float nextPosY = auxPos.x + _currentFishVelocity * Time.deltaTime;
        if (nextPosY > _miniGameTopBound - _fishHalfHeight || nextPosY < _miniGameBottomBound + _fishHalfHeight)
            _currentFishVelocity *= -1;
        auxPos.x = auxPos.x + _currentFishVelocity * Time.deltaTime;
        //auxPos.y = Mathf.Clamp(auxPos.y + _currentFishVelocity * Time.deltaTime, _miniGameBottomBound + _fishHalfHeight, _miniGameTopBound - _fishHalfHeight);
        //auxPos.y = Mathf.PingPong(auxPos.y + _currentFishVelocity * Time.deltaTime, _miniGameTopBound-_miniGameBottomBound - 2*_fishHalfHeight) +
        //           _miniGameBottomBound + _fishHalfHeight;
        fishIcon.position = auxPos;
        auxPos = controlledBar.position;
        auxPos.x = Mathf.Clamp(auxPos.x + _currentBarVelocity * Time.deltaTime,
            _miniGameBottomBound + _barHalfHeight, _miniGameTopBound - _barHalfHeight);
        controlledBar.position = auxPos;
    }

    private void SetProgressBarSize()
    {
        Vector3 auxVector = progressBar.localScale;
        auxVector.y = _progressBarFullHeight * _currentProgress;
        progressBar.localScale = auxVector;
        auxVector = progressBar.localPosition;
        auxVector.y = Mathf.Lerp(-_progressBarFullHeight / 2, 0, _currentProgress);
        progressBar.localPosition = auxVector;
    }

    private void CheckForProgress()
    {
        if (controlledBar.position.x > fishIcon.position.x + _barHalfHeight + _fishHalfHeight ||
            controlledBar.position.x < fishIcon.position.x - _barHalfHeight - _fishHalfHeight)
        {
            _currentProgress = Mathf.Clamp(_currentProgress - progressSpeed * Time.deltaTime, 0, 1);
        }
        else
        {
            _currentProgress = Mathf.Clamp(_currentProgress + progressSpeed * Time.deltaTime, 0, 1);
            if (_currentProgress == 1)
            {
                TaskSuccessful();
            }
        }
        SetProgressBarSize();
    }
    #endregion

    protected override void TaskSuccessful()
    {
        base.TaskSuccessful();
        UI.SetActive(false);
        Debug.Log("Pesca bem sucedida");
    }
    protected override void TaskMistakeLeave()
    {
        base.TaskMistakeLeave();
        UI.SetActive(false);
    }
}
