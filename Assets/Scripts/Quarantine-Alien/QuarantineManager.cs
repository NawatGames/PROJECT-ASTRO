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

    private RoomQuarantineHandler activeRoom; // Referência para a sala atualmente em quarentena

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

    public bool CanActivateQuarantine(RoomQuarantineHandler roomScript)
    {
           
        if (activeRoom == null || activeRoom == roomScript)
        {
            return true;
        }
        else
        {
            return false;
        }
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
