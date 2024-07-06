using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AlienBehavior : MonoBehaviour
{
    public GameObject roomManager;
    public List<GameObject> roomsToInvade;

    [SerializeField] private float _timerAlienInvasion;
    [SerializeField] private float _timerInvasionDelay;

    public GameObject roomInvaded;
    private bool _canCheckRooms;

    public UnityEvent gameOverEvent;
    public UnityEvent alienQuarantinedEvent;
    // Start is called before the first frame update
    void Start()
    {
        _canCheckRooms = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_canCheckRooms)
        {
            _canCheckRooms = false;
            StartCoroutine(RoomsAvailable());
        }

    }
    private IEnumerator RoomsAvailable()
    {

        QuarantineManager manager = roomManager.GetComponent<QuarantineManager>();

        yield return new WaitForSecondsRealtime(_timerInvasionDelay);
        Debug.Log("Alien can invade!");
        roomsToInvade = manager.roomsBeingUsed;
        _canCheckRooms = true;
        StartCoroutine(InvasionStart());
    }
    private IEnumerator InvasionStart()
    {
        if (roomsToInvade.Count != 0)
        {
            int roomIndex = Random.Range(0, roomsToInvade.Count - 1);
            Debug.Log(roomIndex);
            roomInvaded = roomsToInvade[roomIndex];
            yield return new WaitForSecondsRealtime(_timerAlienInvasion);
            QuarantineHandler roomInvadedScript = roomInvaded.GetComponent<QuarantineHandler>();
            if (roomInvadedScript.isRoomQuarantined)
            {
                Debug.Log("Alien Quarantined");
                alienQuarantinedEvent.Invoke();

            }
            else
            {
                Debug.Log("GAME OVER");
                gameOverEvent.Invoke();

            }

        }
        else
        {
            Debug.Log("No room to invade");

        }
    }
    void OnEnable()
    {

    }
}
