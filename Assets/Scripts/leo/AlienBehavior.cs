using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AlienBehavior : MonoBehaviour
{
    public QuarantineManager quarantineManager;
    public List<GameObject> roomsToInvade;

    [SerializeField] private float timerAlienInvasion;
    [SerializeField] private float[] invasionDelayPerLevel;
    private float _timerInvasionDelay;

    public GameObject roomInvaded;
    private bool _canCheckRooms;

    public UnityEvent gameOverEvent;
    public UnityEvent alienQuarantinedEvent;
    // Start is called before the first frame update
    void Start()
    {
        _timerInvasionDelay = invasionDelayPerLevel[LevelManager.level];
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
            Debug.Log("Room found! Alien Invading...");
            roomInvaded = roomsToInvadeWeighted[roomIndex];
            yield return new WaitForSecondsRealtime(timerAlienInvasion);
            QuarantineHandler roomInvadedScript = roomInvaded.GetComponent<QuarantineHandler>();
            if (roomInvadedScript.isRoomQuarantined)
            {
                Debug.Log("Alien Quarantined");
                alienQuarantinedEvent.Invoke();
                roomInvadedScript.task.ResetMistakes();
                // TODO Implementar tempo que o alien fica na sala e executar a linha acima quando o tempo acabar
            }
            else
            {
                Debug.Log("GAME OVER");
                gameOverEvent.Invoke();

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
