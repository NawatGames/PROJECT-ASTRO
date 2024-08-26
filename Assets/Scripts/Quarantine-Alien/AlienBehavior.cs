using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AlienBehavior : MonoBehaviour
{
    public QuarantineManager quarantineManager;
    public List<GameObject> roomsToInvade;

    [SerializeField] private LevelParameters[] levelParams;
    private float _timerInvasionDelay;

    public GameObject roomInvaded;
    private bool _canCheckRooms;

    [SerializeField] private GameEvent gameOverEvent;
    [SerializeField] private GameEvent alienWarningStartEvent;
    [SerializeField] private GameEvent alienWarningEndEvent;

    void Start()
    {
        _timerInvasionDelay = levelParams[LevelManager.level].invasionDelaySeconds;
        StartCoroutine(WaitAndActivateAlien());
    }

    void Update()
    {
        // Verifica se existem salas com players dentro  
        if (_canCheckRooms)
        {
            _canCheckRooms = false;
            StartCoroutine(RoomsAvailable());
        }

    }

    private IEnumerator WaitAndActivateAlien()
    {
        yield return new WaitForSeconds(levelParams[LevelManager.level].alienInactiveAtStartSeconds);
        Debug.Log("Alien awoke");
        _canCheckRooms = true;
    }
    
    private IEnumerator RoomsAvailable()
    {
        yield return new WaitForSecondsRealtime(_timerInvasionDelay);
        Debug.Log("Alien is looking for rooms!");
        roomsToInvade = quarantineManager.roomsBeingUsed;

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
        List<GameObject> roomsToInvadeWeighted = new List<GameObject>(roomsToInvade);
        foreach (GameObject room in roomsToInvade)
        {
            for (int i = 0; i < quarantineManager.roomToTask[room].Mistakes; i++)
            {
                roomsToInvadeWeighted.Add(room);
            }
        }
        int roomIndex = Random.Range(-1, roomsToInvadeWeighted.Count);
        // Debug.Log(roomIndex);
        if (roomIndex != -1)
        {
            roomInvaded = roomsToInvadeWeighted[roomIndex];
            RoomQuarantineHandler roomInvadedScript = roomInvaded.GetComponent<RoomQuarantineHandler>();
            alienWarningStartEvent.Raise(roomInvaded.transform);
            FindObjectOfType<AudioManager>().Play("AlienCrawl");
            yield return new WaitForSecondsRealtime(levelParams[LevelManager.level].invasionWarningSeconds);
            alienWarningEndEvent.Raise(roomInvaded.transform);

            FindObjectOfType<AudioManager>().Stop("AlienCrawl");

            if (roomInvadedScript.isRoomQuarantined && !roomInvadedScript.isBeingUsed)
            {
                //Debug.Log("Alien Quarantined");
                StartCoroutine(roomInvadedScript.AlienIsInsideTimer(levelParams[LevelManager.level].alienInsideSeconds));
                roomInvadedScript.task.ResetMistakes();
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("VentOpened");
                gameOverEvent.Raise();
            }
        }
        else
        {
            // Alien falhou em invadir uma sala 
            Debug.Log("No room invaded");
        }
        _canCheckRooms = true;
    }
}
