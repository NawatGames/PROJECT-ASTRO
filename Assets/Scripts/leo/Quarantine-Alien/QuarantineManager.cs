using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuarantineManager : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<QuarantineHandler> roomsScript;
    public List<GameObject> roomsBeingUsed;

    // public UnityEvent roomQuaratined;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject room in rooms)
        {
            roomsScript.Add(room.GetComponent<QuarantineHandler>());
        }


    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> roomsInUse = new List<GameObject>();
        foreach (GameObject room in rooms)
        {
            QuarantineHandler script = room.GetComponent<QuarantineHandler>();
            if (script.isBeingUsed && !roomsInUse.Any(x => x == room))
            {
                roomsInUse.Add(room);
            }
        }
        this.roomsBeingUsed = roomsInUse;
    }
    public void DisableQuaratines(QuarantineHandler roomQuaratinedScript)
    {
        foreach (QuarantineHandler script in roomsScript)
        {
            if (script != roomQuaratinedScript)
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
