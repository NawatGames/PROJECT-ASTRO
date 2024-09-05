using UnityEngine;
using UnityEngine.Events;

namespace Player.StateMachine
{
    public class NewPlayerState : MonoBehaviour
    {
        public UnityEvent stateEnterEvent;
        public UnityEvent stateLeaveEvent;

        public void EnterState()
        {
            stateEnterEvent.Invoke();
        }

        public void LeaveState()
        {
            stateLeaveEvent.Invoke();
        }
    }
}