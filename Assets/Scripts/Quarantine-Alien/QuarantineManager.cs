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
    private List<RoomQuarantineHandler> closedDoorsRooms = new List<RoomQuarantineHandler>();

    private List<RoomQuarantineHandler> roomNotQuarantinable = new List<RoomQuarantineHandler>();
    
    private int closedDoors;

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
        foreach (RoomQuarantineHandler script in roomsScript)
        {
            if (script != roomQuarantinedScript)
            {
                script.isRoomQuarantined = false;
                script.canPressButton = false;
            }
        }
    }
    public void EnableQuarantines()
    {
        foreach (RoomQuarantineHandler script in roomsScript)
        {
            script.isRoomQuarantined = false;
            script.canPressButton = true;
        }
    }
    
    public float getTimerQuarantineDelay()
    {
        return timerQuarantineDelay;
    }
    
}
