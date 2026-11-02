using System.Collections.Generic;
using UnityEngine;

public class QuarantineManager : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<RoomQuarantineHandler> roomsScript;
    public Dictionary<GameObject, TaskController> roomToTask; // Para o 'alienBehavior' saber qual a task do quarto invadido e seus 'mistakes'
    public List<GameObject> roomsBeingUsed;
    [SerializeField] private int timerQuarantineDelay;

    [SerializeField] private GameEvent quarantineActivated;
    [SerializeField] private GameEvent quarantineDeactivated;

    private MainDoorButtonController[] _doorButtons; // Mesmo índice de 'rooms'
    private readonly List<RoomQuarantineHandler> _openRooms = new List<RoomQuarantineHandler>();

    private void Start()
    {
        roomToTask = new Dictionary<GameObject, TaskController>();
        roomsScript.Clear();
        _doorButtons = new MainDoorButtonController[rooms.Count];
        for (int i = 0; i < rooms.Count; i++)
        {
            RoomQuarantineHandler script = rooms[i].GetComponent<RoomQuarantineHandler>();
            roomsScript.Add(script);
            roomToTask.Add(rooms[i], script.task);
            _doorButtons[i] = rooms[i].GetComponentInChildren<MainDoorButtonController>();
        }
    }

    // Mantém o estado das salas coerente com as portas a cada frame:
    // - porta principal fechada => sala em quarentena (e todas as portas dela fechadas)
    // - só uma sala pode ficar em quarentena; uma segunda sala fechada é reaberta
    // - enquanto houver quarentena, as salas abertas não podem ser quarentenadas
    private void Update()
    {
        List<GameObject> roomsInUse = new List<GameObject>(); // Nova lista: o AlienBehavior guarda a referência
        int closedDoors = 0;
        int notQuarantinableRoomsCount = 0;
        _openRooms.Clear();

        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject room = rooms[i];
            RoomQuarantineHandler script = roomsScript[i];
            MainDoorButtonController doorButton = _doorButtons[i];

            if (script.isBeingUsed)
            {
                roomsInUse.Add(room);
            }

            if (!doorButton.IsDoorOpen())
            {
                closedDoors++;
                script.isRoomQuarantined = true;
                doorButton.CloseAllRoomDoors();
            }

            if (!script.canPressButton && !script.isRoomQuarantined)
            {
                notQuarantinableRoomsCount++;
            }

            if (doorButton.IsDoorOpen())
            {
                _openRooms.Add(script);
            }

            // Failsafe: se nenhuma sala está quarentenada nem pode ser, reabre as portas
            if (notQuarantinableRoomsCount >= rooms.Count)
            {
                doorButton.OpenAllRoomDoors();
                notQuarantinableRoomsCount = 0;
            }

            if (closedDoors > 1)
            {
                doorButton.OpenAllRoomDoors();
                script.isRoomQuarantined = false;
                script.canPressButton = false;
                script.quarantineEnded.Invoke();
                closedDoors = 1;
            }
        }

        if (closedDoors >= 1)
        {
            foreach (RoomQuarantineHandler openRoom in _openRooms)
            {
                openRoom.isRoomQuarantined = false;
                openRoom.canPressButton = false;
            }
        }

        roomsBeingUsed = roomsInUse;
    }

    // Ativa a quarentena (desabilita a opção de quarentenar)
    public void DisableQuarantines(RoomQuarantineHandler roomQuarantinedScript)
    {
        foreach (RoomQuarantineHandler script in roomsScript)
        {
            script.canPressButton = false;
            if (script != roomQuarantinedScript)
            {
                script.isRoomQuarantined = false;
            }
        }
        quarantineActivated.Raise();
    }

    // Desativa a quarentena (habilita a opção de quarentenar)
    public void EnableQuarantines(RoomQuarantineHandler openedRoomScript)
    {
        foreach (RoomQuarantineHandler script in roomsScript)
        {
            script.isRoomQuarantined = false;
            if (script != openedRoomScript)
            {
                script.canPressButton = true;
            }
        }
        quarantineDeactivated.Raise();
    }

    public float getTimerQuarantineDelay()
    {
        return timerQuarantineDelay;
    }
}
