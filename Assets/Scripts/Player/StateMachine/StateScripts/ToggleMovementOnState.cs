using System;
using Player.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.StateMachine.StateScripts
{
    public class ToggleMovementOnState : BaseStateScript
    {
        [SerializeField] private PlayerMovementController playerMovementController;
        
        // Enable player movement on state enter
        protected override void EnterState()
        {
            playerMovementController.enabled = true;
        }

        // Disable player movement on state leave
        protected override void LeaveState()
        {
            playerMovementController.enabled = false;
        }
    }
}