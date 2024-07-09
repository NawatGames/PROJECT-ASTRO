using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public Transform astroSpawn;
    public Transform alienSpawn;
    
    void Awake()
    {
        var inputManager = GetComponent<PlayerInputManager>();
        GameObject astronauta = PlayerInput.Instantiate(playerPrefab, 0, "LeftKeyboard", default, Keyboard.current).gameObject;
        astronauta.transform.position = astroSpawn.position;
        GameObject alien = PlayerInput.Instantiate(playerPrefab, 1, "RightKeyboard", default, Keyboard.current).gameObject;
        alien.GetComponent<PlayerController>().isAstro = false;
        alien.transform.position = alienSpawn.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
