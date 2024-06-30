using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarantineHandler : MonoBehaviour
{
    [SerializeField] float timerA;
    public List<GameObject> roomGroup;
    public List<GameObject> roomAvailableToInvade;
    public List<bool> isRoomOcuppied;

    // Start is called before the first frame update
    void Start()
    {
        isRoomOcuppied = new List<bool>(3) { true, false, true };
    }

    // Update is called once per frame
    void Update()
    {

        int i = 0;
        foreach (GameObject room in roomGroup)
        {
            // if(room.isOccupied)
            // {
            //     isRoomOcuppied[i] = true;
            // }
            // else isRoomOcuppied[i] = false;
            // i++;

        }
        int j = 0;
        foreach (bool roomState in isRoomOcuppied)
        {
            if(roomState == true)
            {
                foreach (GameObject room in roomAvailableToInvade)
                {
                    if(room.GetInstanceID() == roomAvailableToInvade[j].GetInstanceID())
                    {

                    }
                    else
                    {
                    }
                }
                roomAvailableToInvade.Add(roomGroup[j]);
            }
            j++;
        }
    }
}
