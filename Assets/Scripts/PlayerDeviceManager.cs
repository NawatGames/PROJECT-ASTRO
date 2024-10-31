using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeviceManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerInput astroPlayerInput;
    [SerializeField] private PlayerInput alienPlayerInput;
    private const string AstroControlScheme = "LeftKeyboard";
    private const string AlienControlScheme = "RightKeyboard";
    
    private void Awake()
    {
        int playerLayerMask = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(playerLayerMask, playerLayerMask);
    }

    private void OnEnable()
    {
        astroPlayerInput.SwitchCurrentControlScheme(AstroControlScheme, Keyboard.current);
        alienPlayerInput.SwitchCurrentControlScheme(AlienControlScheme, Keyboard.current);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(astroSpawn, "d_AvatarSelector", true, astroSpawnIconColor);
        Gizmos.DrawIcon(alienSpawn, "d_AvatarSelector", true, alienSpawnIconColor);
    }
    */
}
