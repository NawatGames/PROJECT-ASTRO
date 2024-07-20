using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AlienBehavior : MonoBehaviour
{
    public GameObject roomManager;
    public List<GameObject> roomsToInvade;

    [SerializeField] private float timerAlienInvasion;
    [SerializeField] private float timerInvasionDelay;

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
        // Verifica se existem salas com players dentro  
        if (_canCheckRooms)
        {
            _canCheckRooms = false;
            StartCoroutine(RoomsAvailable());
        }

    }
    private IEnumerator RoomsAvailable()
    {

        QuarantineManager manager = roomManager.GetComponent<QuarantineManager>();
        yield return new WaitForSecondsRealtime(timerInvasionDelay);
        Debug.Log("Alien is looking for rooms!");
        roomsToInvade = manager.roomsBeingUsed;

        if (roomsToInvade.Count != 0)
        {
            // Alien tenta invadir uma sala
            StartCoroutine(InvasionStart());
        }
        else
        {
            Debug.Log("No room to invade");
            _canCheckRooms = true;

        }
    }
    private IEnumerator InvasionStart()
    {
        int roomIndex = Random.Range(-1, roomsToInvade.Count);
        // Debug.Log(roomIndex);
        if (roomIndex != -1)
        {
            Debug.Log("Room found! Alien Invading...");
            roomInvaded = roomsToInvade[roomIndex];
            yield return new WaitForSecondsRealtime(timerAlienInvasion);
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
        else if (roomIndex == -1)
        {
            // Alien falhou em invadir uma sala 
            Debug.Log("No room invaded");
        }
        _canCheckRooms = true;
    }

}
