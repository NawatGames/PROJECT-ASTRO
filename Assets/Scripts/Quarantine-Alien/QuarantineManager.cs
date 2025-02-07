using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuarantineManager : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<RoomQuarantineHandler> roomsScript;
    public Dictionary<GameObject, TaskController> roomToTask; // Para o 'alienBehavior' saber qual a task do quarto invadido e seus 'mistakes'
    public List<GameObject> roomsBeingUsed;
    [SerializeField] private int timerQuarantineDelay;


    [SerializeField] private List<GameObject> linkedRooms;

    [SerializeField] private GameEvent quarantineActivated;
    [SerializeField] private GameEvent quarantineDeactivated;
    
    private int closedDoors;

    private int notQuarantinableRoomsCount;
    // public UnityEvent roomQuarantined;
    
   
    
    private void Start()
    {
        roomToTask = new Dictionary<GameObject, TaskController>();
        foreach (GameObject room in rooms)
        {
            RoomQuarantineHandler script = room.GetComponent<RoomQuarantineHandler>();
            roomsScript.Add(script);
            roomToTask.Add(room, script.task);
        }

        
    }
    
    private void Update()
    {
        List<GameObject> roomsInUse = new List<GameObject>();
        closedDoors = 0;
        notQuarantinableRoomsCount = 0;
        List<RoomQuarantineHandler> roomNotQuarantinable = new List<RoomQuarantineHandler>();
        foreach (GameObject room in rooms)
        {
            RoomQuarantineHandler script = room.GetComponent<RoomQuarantineHandler>();
            DoorButtonController doorButton = room.GetComponentInChildren<DoorButtonController>();
            if (script.isBeingUsed && !roomsInUse.Contains(room))
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
            
            if (doorButton.IsDoorOpen() )//&& adjacentDoorButton.IsDoorOpen())
            {
                roomNotQuarantinable.Add(script);
            }

            if (notQuarantinableRoomsCount >= 8)
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
            foreach (RoomQuarantineHandler rooms1 in roomNotQuarantinable)
            {
                
                rooms1.isRoomQuarantined = false;
                rooms1.canPressButton = false;
                
            }

        }
        roomNotQuarantinable.Clear();

        this.roomsBeingUsed = roomsInUse;
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
