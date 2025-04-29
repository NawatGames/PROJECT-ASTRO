using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class RoomQuarantineHandler : MonoBehaviour
{
    public TaskController task;

    public QuarantineManager manager;
    [SerializeField] public bool canPressButton;

    [SerializeField] public bool isBeingUsed;
    private bool _isBeingUsedTwice;

    private UnityEvent<bool> _onIsUsingRoomChanged;
    public UnityEvent quarantineStarted;
    public UnityEvent quarantineEnded;
    [SerializeField] public bool isRoomQuarantined;

    private bool _isAlienInside;
    [SerializeField] private GameEvent onAlienAttack;
    [SerializeField] private GameEvent buttonCooldownEnded;

    // public GameObject room;
    // public SpriteRenderer roomSprite;
    public SpriteRenderer wallSprite;

    [SerializeField][Range(0, 1)] private float fadeVel = 0.03f;

    [SerializeField] private Animator buttonAnimator;


    // pegar o tempo do alien para a quarentena
    
    // luz para os bottoes
    [SerializeField]private Light2D buttonLight;
    private Coroutine _blinkingCoroutine;

    void Start()
    {
        canPressButton = true;
        
    }

    void Update()
    {
        RoomColorDebug();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isBeingUsed)
                _isBeingUsedTwice = true;
            else
            {
                isBeingUsed = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isBeingUsedTwice)
                _isBeingUsedTwice = false;
            else
            {
                isBeingUsed = false;
            }
        }
    }

    private void RoomColorDebug()
    {
        if (_isAlienInside)
        {
            // roomSprite.color = Color.black;
            if (_blinkingCoroutine == null)
                _blinkingCoroutine = StartCoroutine(BlinkButtonLight());
        }
        else if (isRoomQuarantined)
        {
            //Sala quarentenada
            // roomSprite.color = Color.red;
            if (_blinkingCoroutine == null)
            {
                buttonLight.color = Color.red;
                buttonLight.intensity = 6;  
            }
            
            
            buttonLight.pointLightOuterRadius = 1;
            if (wallSprite.color.a < 1) wallSprite.color = new Color(0, 0, 0, wallSprite.color.a + fadeVel);

            buttonAnimator.SetBool("Unpressable", false);
        }
        else if (!canPressButton && !isRoomQuarantined)
        {
            //Sala que nao pode ser quarentenada
            // roomSprite.color = Color.blue;
            // yellow
            buttonLight.color = Color.yellow;
            buttonLight.intensity = 3;
            buttonLight.pointLightOuterRadius = 0.4f;
            if (wallSprite.color.a > 0) wallSprite.color = new Color(0, 0, 0, wallSprite.color.a - fadeVel);

            buttonAnimator.SetBool("IsPressed", false);
            buttonAnimator.SetBool("Unpressable", true);
        }
        else
        {
            buttonAnimator.SetBool("Unpressable", false);
            buttonLight.color = Color.green;
            buttonLight.intensity = 3;
            // roomSprite.color = new Color(0.75f, 1, 1, 0.0275f);
            if (wallSprite.color.a > 0) wallSprite.color = new Color(0, 0, 0, wallSprite.color.a - fadeVel);
            if (_blinkingCoroutine != null)
            {
                StopCoroutine(_blinkingCoroutine);
                _blinkingCoroutine = null;
                Debug.Log("Stopping blinking coroutine");
            }
            
        }

       
    }
    
    private IEnumerator BlinkButtonLight()
    {
        while(true)
        {
            buttonLight.color = Color.red;
            buttonLight.intensity = 8;
            buttonLight.pointLightOuterRadius = 1.5f;
            yield return new WaitForSeconds(0.5f); // Light on

           
            buttonLight.color = Color.black;
            buttonLight.intensity = 0;
            yield return new WaitForSeconds(0.5f); // Light off
            
        }
    }
    private IEnumerator QuarantineToggleRoutine()
    {
        if (!isRoomQuarantined)
        {
            isRoomQuarantined = true;
            quarantineStarted.Invoke();
            manager.DisableQuarantines(this);
        }
        else if (isRoomQuarantined)
        {
            if (_isAlienInside)
            {
                onAlienAttack.Raise();
            }
            isRoomQuarantined = false;
            canPressButton = false;
            quarantineEnded.Invoke();
            manager.EnableQuarantines(this);


        }
        StartCoroutine(QuarantineDelay());
        yield return null;
    }
    private IEnumerator QuarantineDelay()
    {
        yield return new WaitForSeconds(manager.getTimerQuarantineDelay());
        canPressButton = true;
        buttonCooldownEnded.Raise(this);
    }

    public void ToggleQuarantine()
    {
        StartCoroutine(QuarantineToggleRoutine());
    }

    public IEnumerator AlienIsInsideTimer(float alienInsideSeconds)
    {
        _isAlienInside = true;
        yield return new WaitForSeconds(alienInsideSeconds);
        _isAlienInside = false;
    }
}
