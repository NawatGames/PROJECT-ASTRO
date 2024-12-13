using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] public PlayerInput input;
    [HideInInspector] public PlayerInputAsset inputAsset; //TODO: Manter apenas este ou os dois debaixo
    [HideInInspector] public InputAction movementInputAction;
    [HideInInspector] public InputAction interactionInputAction;
    [HideInInspector] public InputAction pauseInputAction;
    
    private void Awake()
    {
        inputAsset = new PlayerInputAsset();
        input.actions = inputAsset.asset;
        movementInputAction = inputAsset.Default.Movement;
        interactionInputAction = inputAsset.Default.Interaction;
        pauseInputAction = inputAsset.Default.Pause;
    }
    
    
}
