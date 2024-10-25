using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [Header("Customization")] 
    [SerializeField] private Color astroSpawnIconColor;
    [SerializeField] private Color alienSpawnIconColor;
    [Header("Config")]
    public Vector2 astroSpawn;
    public Vector2 alienSpawn;
    [SerializeField] private GameObject astroPrefab;
    [SerializeField] private GameObject alienPrefab;

    private const string AstroControlScheme = "LeftKeyBoard";
    private const string AlienControlScheme = "RightKeyBoard";
    
    
    private void Awake()
    {
        int playerLayerMask = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(playerLayerMask, playerLayerMask);
        GameObject astro = PlayerInput.Instantiate(astroPrefab, 0, AstroControlScheme, default, Keyboard.current).gameObject;
        astro.transform.position = astroSpawn;
        GameObject alien = PlayerInput.Instantiate(alienPrefab, 1, AlienControlScheme, default, Keyboard.current).gameObject;
        alien.transform.position = alienSpawn;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(astroSpawn, "d_AvatarSelector", true, astroSpawnIconColor);
        Gizmos.DrawIcon(alienSpawn, "d_AvatarSelector", true, alienSpawnIconColor);
    }
}
