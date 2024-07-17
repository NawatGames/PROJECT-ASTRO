using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuarantineManager : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<QuarantineHandler> roomsScript;
    public List<GameObject> roomsBeingUsed;

    // public UnityEvent roomQuarantined;


    // Start is called before the first frame update
    private void Start()
    {
        foreach (GameObject room in rooms)
        {
            roomsScript.Add(room.GetComponent<QuarantineHandler>());
        }


    }

    // Update is called once per frame
    private void Update()
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
