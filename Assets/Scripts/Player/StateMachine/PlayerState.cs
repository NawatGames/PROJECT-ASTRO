using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.StateMachine
{
    public abstract class PlayerState : MonoBehaviour
    {
        protected PlayerStateMachine playerStateMachine;
        protected PlayerInputController playerInputController;
        
        public UnityEvent stateEnterEvent;
        public UnityEvent stateLeaveEvent;

        protected virtual void Awake()
        {
            playerStateMachine = transform.parent.GetComponent<PlayerStateMachine>();
            playerInputController = transform.root.GetComponentInChildren<PlayerInputController>();
        }

        public virtual void EnterState()
        {
            stateEnterEvent.Invoke();
            playerInputController.interactionInputAction.performed += OnInteractHandler;
            
        }

        public virtual void LeaveState()
        {
            stateLeaveEvent.Invoke();
            playerInputController.interactionInputAction.performed -= OnInteractHandler;
        }

        protected void SwitchState(PlayerState newState)
        {
            playerStateMachine.SetState(newState);
        }

        public abstract void StateUpdate();
        
        protected abstract void OnInteractHandler(InputAction.CallbackContext ctx);
    }
}