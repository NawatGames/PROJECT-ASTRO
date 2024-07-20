using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class QuarantineHandler : MonoBehaviour
{
    public QuarantineManager manager;
    [SerializeField] private float timerQuarantineDelay;
    [SerializeField] public bool canPressButton;

    [SerializeField] public bool isBeingUsed;

    public UnityEvent quarantineStarted;
    public UnityEvent quarantineEnded;
    [SerializeField] public bool isRoomQuarantined = false;

    // public GameObject room;
    public SpriteRenderer roomSprite;

    // Start is called before the first frame update
    void Start()
    {
        canPressButton = true;
        // roomQuarantined.AddListener();
    }

    // Update is called once per frame
    void Update()
    {
        RoomColorDebug();

    }

    private void RoomColorDebug()
    {
        if (isRoomQuarantined)
        {
            //Sala quarentenada
            roomSprite.color = Color.red;
        }
        else if (!canPressButton && !isRoomQuarantined)
        {
            //Sala que nao pode ser quarentenada
            roomSprite.color = Color.blue;
        }
        else roomSprite.color = Color.white;
    }

    private IEnumerator QuarantineToggle()
    {
        if (!isRoomQuarantined)
        {
            isRoomQuarantined = true;
            quarantineStarted.Invoke();
        }
        else if (isRoomQuarantined)
        {
            isRoomQuarantined = false;
            quarantineEnded.Invoke();
        }
        StartCoroutine(QuarantineDelay());
        yield return null;
    }
    private IEnumerator QuarantineDelay()
    {
        yield return new WaitForSecondsRealtime(timerQuarantineDelay);
        canPressButton = true;
    }

    private void OnQuarantineStarted()
    {
        if (canPressButton)
        {
            StartCoroutine(QuarantineToggle());
            canPressButton = false;
        }
    }
}
