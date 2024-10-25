using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [HideInInspector] public PlayerInputAsset inputAsset; //TODO: Manter apenas este ou os dois debaixo
    [HideInInspector] public InputAction movementInputAction;
    [HideInInspector] public InputAction interactionInputAction;
    
    private void Awake()
    {
        inputAsset = new PlayerInputAsset();
        input.actions = inputAsset.asset;
        movementInputAction = inputAsset.Default.Movement;
        interactionInputAction = inputAsset.Default.Interaction;
    }
    
    
}
