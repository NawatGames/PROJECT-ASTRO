using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuarantineManager : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<QuarantineHandler> roomsScript;
    public Dictionary<GameObject, TaskController> roomToTask; // Para o 'alienBehavior' saber qual a task do quarto invadido e seus 'mistakes'
    public List<GameObject> roomsBeingUsed;

    // public UnityEvent roomQuarantined;
    
    private void Start()
    {
        roomToTask = new Dictionary<GameObject, TaskController>();
        foreach (GameObject room in rooms)
        {
            QuarantineHandler script = room.GetComponent<QuarantineHandler>();
            roomsScript.Add(script);
            roomToTask.Add(room, script.task);
        }
    }
    
    private void Update()
    {
        List<GameObject> roomsInUse = new List<GameObject>();
        foreach (GameObject room in rooms)
        {
            QuarantineHandler script = room.GetComponent<QuarantineHandler>();
            if (script.isBeingUsed && roomsInUse.All(x => x != room))
            {
                roomsInUse.Add(room);
            }
        }
        this.roomsBeingUsed = roomsInUse;
    }
    public void DisableQuarantines(QuarantineHandler roomQuarantinedScript)
    {
        foreach (QuarantineHandler script in roomsScript)
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
        foreach (QuarantineHandler script in roomsScript)
        {
            script.isRoomQuarantined = false;
            script.canPressButton = true;
        }
    }
}
