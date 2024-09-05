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
    [SerializeField] public float timerQuarantineDelay;

    private void Start()
    {
        roomToTask = new Dictionary<GameObject, TaskController>();
        foreach (GameObject room in rooms)
        {
            RoomQuarantineHandler script = room.GetComponent<RoomQuarantineHandler>();
            roomsScript.Add(script);
            roomToTask.Add(room, script.task);
        }
        isAnyRoomInCooldown = false; // nenhuma sala está em cooldown
    }

    private void Update()
    {
        List<GameObject> roomsInUse = new List<GameObject>();
        foreach (GameObject room in rooms)
        {
            RoomQuarantineHandler script = room.GetComponent<RoomQuarantineHandler>();
            if (script.isBeingUsed && !roomsInUse.Contains(room))
            {
                roomsInUse.Add(room);
            }
        }
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
        isAnyRoomInCooldown = true; // Uma sala está em cooldown
        
    }

    public void EnableQuarantines()
    {
        foreach (RoomQuarantineHandler script in roomsScript)
        {
            script.isRoomQuarantined = false;
            script.canPressButton = true;
        }
        isAnyRoomInCooldown = false; // Nenhuma sala está em cooldown
       
    }
    
    // public void CheckAllRooms()
    // {
    //     int i = 0;
    //     foreach (RoomQuarantineHandler script in roomsScript)
    //     {
    //         if (script.isRoomQuarantined)
    //         {
    //             script.roomSprite.color = Color.red;
    //             i++;
    //         }
    //         else
    //         {
    //             script.roomSprite.color = Color.blue;
    //         }
    //     }
    //
    //     if (i > 1)
    //     {
    //         foreach (RoomQuarantineHandler script in roomsScript)
    //         {   
    //             script.isRoomQuarantined = false;
    //             if (script.isRoomQuarantined)
    //             {
    //                 script.roomSprite.color = Color.red;
    //             }
    //             else
    //             {
    //                 script.roomSprite.color = Color.blue;
    //             }
    //         }
    //     }
    // }
}