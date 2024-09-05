using System;
using Player.Movement;
using Player.StateMachine;
using UnityEngine;

namespace Player.StateMachine.StateScripts
{
    public class ToggleMovementOnState : BaseStateScript
    {
        [SerializeField] private PlayerMovement playerMovement;
        protected override void EnterState()
        {
            playerMovement.enabled = true;
        }

        protected override void LeaveState()
        {
            playerMovement.enabled = false;
        }
    }
}