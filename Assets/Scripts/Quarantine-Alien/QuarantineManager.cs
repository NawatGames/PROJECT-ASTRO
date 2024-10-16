using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuarantineManager : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<RoomQuarantineHandler> roomsScript;
    public Dictionary<GameObject, TaskController> roomToTask;
    public List<GameObject> roomsBeingUsed;
    public bool isAnyRoomInCooldown;
    //[SerializeField] public float timerQuarantineDelay;
    private List<RoomQuarantineHandler> closedDoorsRooms = new List<RoomQuarantineHandler>();

    private List<RoomQuarantineHandler> roomNotQuarantinable = new List<RoomQuarantineHandler>();

    private RoomQuarantineHandler activeRoom; // Referência para a sala atualmente em quarentena

    private int closedDoors;

    private void Start()
    {
        roomToTask = new Dictionary<GameObject, TaskController>();
        foreach (GameObject room in rooms)
        {
            RoomQuarantineHandler script = room.GetComponent<RoomQuarantineHandler>();
            roomsScript.Add(script);
            roomToTask.Add(room, script.task);
        }
        isAnyRoomInCooldown = false; // Nenhuma sala está em cooldown
        activeRoom = null;
    }

    private void Update()
    {
        List<GameObject> roomsInUse = new List<GameObject>();
        closedDoors = 0;
        DoorButtonController previousDoorButton = null;
        
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
                
            }
            
            if (doorButton.IsDoorOpen())
            {
                roomNotQuarantinable.Add(script);
            }

            
            if (closedDoors > 1)
            {
                
                 doorButton.OpenDoor();  
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
    

    public void DisableQuarantines(RoomQuarantineHandler roomQuarantinedScript)
    {
        activeRoom = roomQuarantinedScript; // Define a sala ativa
        foreach (RoomQuarantineHandler script in roomsScript)
        {
            if (script != roomQuarantinedScript)
            {
                script.isRoomQuarantined = false;
                script.canPressButton = false;
            }
        }
        isAnyRoomInCooldown = true; // Uma sala está em cooldown
    }

    public void EnableQuarantines()
    {
        activeRoom = null; // Reseta a sala ativa
        foreach (RoomQuarantineHandler script in roomsScript)
        {
            script.isRoomQuarantined = false;
            script.canPressButton = true;
        }
        isAnyRoomInCooldown = false; // Nenhuma sala está em cooldown
    }
}
