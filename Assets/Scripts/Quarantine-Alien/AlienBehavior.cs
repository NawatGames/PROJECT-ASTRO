using System.Collections;
using System.Collections.Generic;
using Audio_System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AlienBehavior : MonoBehaviour
{
    [Header("AUDIO SAMPLES")]
    [SerializeField] private GameObject alienCrawlAudio;
    [SerializeField] private GameObject alienQuarantinedAudio;
    [SerializeField] private GameObject alienOpenVentAudio;


    [Header("ENTITY PARAMETERS")]
    public QuarantineManager quarantineManager;
    public List<GameObject> roomsToInvade;

    [SerializeField] private LevelParameters[] levelParams;
    private float _timerInvasionDelay;

    public GameObject roomInvaded;
    private bool _canCheckRooms;

    [SerializeField] private GameEvent alienAttackEvent;
    [SerializeField] private GameEvent alienWarningStartEvent;
    [SerializeField] private GameEvent alienWarningEndEvent;
    [SerializeField] private GameEvent alienQuarantinedEvent;


    private int _levelIndex;

    void Start()
    {
        _levelIndex = SaveManager.CurrentLevel - 1;
        _timerInvasionDelay = levelParams[_levelIndex].invasionDelaySeconds;
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
        yield return new WaitForSeconds(levelParams[_levelIndex].alienInactiveAtStartSeconds);
        Debug.Log("Alien awoke");
        _canCheckRooms = true;
    }

    private IEnumerator RoomsAvailable()
    {
        yield return new WaitForSeconds(_timerInvasionDelay);
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
            alienCrawlAudio.GetComponent<AudioPlayer>().PlayLoop();
            yield return new WaitForSeconds(levelParams[_levelIndex].invasionWarningSeconds);
            alienCrawlAudio.GetComponent<AudioPlayer>().StopAudio();

            if (roomInvadedScript.isRoomQuarantined && !roomInvadedScript.isBeingUsed)
            {
                //Debug.Log("Alien Quarantined");
                alienQuarantinedEvent.Raise(roomInvaded.transform);
                alienQuarantinedAudio.GetComponent<AudioPlayer>().PlayLoop();

                yield return StartCoroutine(roomInvadedScript.AlienIsInsideTimer(levelParams[_levelIndex].alienInsideSeconds));

                roomInvadedScript.task.ResetMistakes();
                alienWarningEndEvent.Raise(roomInvaded.transform);
                alienQuarantinedAudio.GetComponent<AudioPlayer>().StopAudio();
            }
            else
            {
                alienAttackEvent.Raise(roomInvaded.transform);
                alienOpenVentAudio.GetComponent<AudioPlayer>().PlayAudio();

            }
        }
        else
        {
            // Alien falhou em invadir uma sala 
            Debug.Log("No room invaded");
        }

        _canCheckRooms = true;
        // if (roomInvaded != null)
        // {
        //     alienWarningEndEvent.Raise(roomInvaded.transform);
        // }
    }

}
