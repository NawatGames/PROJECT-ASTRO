using System;
using Player.Movement;
using Player.StateMachine;
using UnityEngine;

namespace Player.StateMachine.StateScripts
{
    public class ToggleMovementOnState : BaseStateScript
    {
        [SerializeField] private PlayerMovement playerMovement;
        
        // Enable player movement on state enter
        protected override void EnterState()
        {
            playerMovement.enabled = true;
        }

        // Disable player movement on state leave
        protected override void LeaveState()
        {
            playerMovement.enabled = false;
        }
    }
}