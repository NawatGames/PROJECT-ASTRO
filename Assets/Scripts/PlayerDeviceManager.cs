using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeviceManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerInput astroPlayerInput;
    [SerializeField] private PlayerInput alienPlayerInput;
    [SerializeField] private GameObject astroPrefab;
    [SerializeField] private GameObject alienPrefab;
    private const string AstroControlScheme = "LeftKeyboard";
    private const string AlienControlScheme = "RightKeyboard";
    
    private void Awake()
    {
        int playerLayerMask = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(playerLayerMask, playerLayerMask);
    }

    private void OnEnable()
    {
        var astro = PlayerInput.Instantiate(prefab:astroPrefab, controlScheme:AstroControlScheme, pairWithDevice:Keyboard.current);
        astro.transform.position = new Vector3(1.25f, -0.4375f, 0);
        var alien = PlayerInput.Instantiate(prefab:alienPrefab, controlScheme:AlienControlScheme, pairWithDevice:Keyboard.current);
        alien.transform.position = new Vector3(3.5625f, -0.5f, 0);
        /*astroPlayerInput.SwitchCurrentControlScheme(AstroControlScheme, Keyboard.current);
        alienPlayerInput.SwitchCurrentControlScheme(AlienControlScheme, Keyboard.current);*/
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(astroSpawn, "d_AvatarSelector", true, astroSpawnIconColor);
        Gizmos.DrawIcon(alienSpawn, "d_AvatarSelector", true, alienSpawnIconColor);
    }
    */
}
