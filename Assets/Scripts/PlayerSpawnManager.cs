using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public Transform astroSpawn;
    public Transform alienSpawn;
    private const string AstroControlScheme = "LeftKeyBoard";
    private const string AlienControlScheme = "RightKeyBoard";
    
    void Awake()
    {
        Physics2D.IgnoreLayerCollision(3, 3); // Layer Mask 3 = "Player"
        GameObject astro = PlayerInput.Instantiate(playerPrefab, 0, AstroControlScheme, default, Keyboard.current).gameObject;
        astro.transform.position = astroSpawn.position;
        GameObject alien = PlayerInput.Instantiate(playerPrefab, 1, AlienControlScheme, default, Keyboard.current).gameObject;
        //alien.GetComponent<PlayerController>().isAstro = false;
        alien.transform.position = alienSpawn.position;
    }
}
