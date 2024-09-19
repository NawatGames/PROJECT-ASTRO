using System;
using UnityEngine;

namespace Player.StateMachine
{
    public class NewPlayerStateMachine : MonoBehaviour
    {
        [SerializeField] private NewPlayerState initialPlayerState;

        private NewPlayerState _currentState;
        private void Start()
        {
            _currentState = initialPlayerState;
            _currentState.EnterState();
        }

        private void SetState(NewPlayerState nextState)
        {
            _currentState.LeaveState();
            _currentState = nextState;
            _currentState.EnterState();
        }
    }
}