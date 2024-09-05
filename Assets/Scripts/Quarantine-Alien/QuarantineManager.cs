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

    private void Start()
    {
        roomToTask = new Dictionary<GameObject, TaskController>();
        foreach (GameObject room in rooms)
        {
            RoomQuarantineHandler script = room.GetComponent<RoomQuarantineHandler>();
            roomsScript.Add(script);
            roomToTask.Add(room, script.task);
        }
        isAnyRoomInCooldown = false; // Variável para controlar cooldown global
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
        roomsBeingUsed = roomsInUse;
    }

    public void DisableQuarantines(RoomQuarantineHandler roomQuarantinedScript)
    {
        foreach (RoomQuarantineHandler script in roomsScript)
        {
            if (script != roomQuarantinedScript)
            {
                script.isRoomQuarantined = false;
                script.canPressButton = false; // Desativa os botões das outras salas
            }
        }
        isAnyRoomInCooldown = true; // Uma sala está em cooldown
    }

    public void EnableQuarantines()
    {
        foreach (RoomQuarantineHandler script in roomsScript)
        {
            script.isRoomQuarantined = false;
            script.canPressButton = true; // Reativa todos os botões
        }
        isAnyRoomInCooldown = false; // Nenhuma sala está em cooldown
    }
}