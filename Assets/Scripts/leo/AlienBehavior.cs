using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBehavior : MonoBehaviour
{
    public GameObject roomManager;
    public List<GameObject> roomsToInvade;

    [SerializeField] private float _timerAlienInvasion;
    [SerializeField] private float _timerInvasionDelay;

    private bool canCheckRooms;
    // Start is called before the first frame update
    void Start()
    {
        canCheckRooms = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCheckRooms)
        {
            canCheckRooms = false;
            StartCoroutine(RoomsAvailable());
        }

    }
    private IEnumerator RoomsAvailable()
    {

        QuarantineManager manager = roomManager.GetComponent<QuarantineManager>();

        yield return new WaitForSecondsRealtime(_timerInvasionDelay);
        Debug.Log("Alien can invade!");
        roomsToInvade = manager.roomsBeingUsed;
        canCheckRooms = true;
    }
    void OnEnable()
    {

    }
}
