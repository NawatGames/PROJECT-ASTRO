using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject astroPrefab;
    [SerializeField] private GameObject alienPrefab;
    public Transform astroSpawn;
    public Transform alienSpawn;
    private const string AstroControlScheme = "LeftKeyBoard";
    private const string AlienControlScheme = "RightKeyBoard";
    
    void Awake()
    {
        int playerLayerMask = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(playerLayerMask, playerLayerMask);
        GameObject astro = PlayerInput.Instantiate(astroPrefab, 0, AstroControlScheme, default, Keyboard.current).gameObject;
        astro.transform.position = astroSpawn.position;
        GameObject alien = PlayerInput.Instantiate(alienPrefab, 1, AlienControlScheme, default, Keyboard.current).gameObject;
        alien.transform.position = alienSpawn.position;
    }
}
