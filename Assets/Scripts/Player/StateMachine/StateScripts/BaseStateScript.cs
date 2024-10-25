using System;
using Player.StateMachine;
using UnityEngine;

namespace Player.StateMachine.StateScripts
{
    /// <summary>
    /// BaseStateScript listens to a NewPlayerState and calls functions when states are entered or left
    /// </summary>
    public abstract class BaseStateScript : MonoBehaviour
    {
        [SerializeField] private PlayerState state;

        protected virtual void OnEnable()
        {
            state.stateEnterEvent.AddListener(EnterState);
            state.stateLeaveEvent.AddListener(LeaveState);
        }

        protected virtual void OnDisable()
        {
            state.stateEnterEvent.RemoveListener(EnterState); 
            state.stateLeaveEvent.RemoveListener(LeaveState);
        }

        protected abstract void EnterState();
        protected abstract void LeaveState();
    }
}