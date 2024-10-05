using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menus.Navigation
{
    public class NavigationManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        [SerializeField] private ButtonNavigation initialButton;
        private ButtonNavigation currentButton;

        private void Awake()
        {
            currentButton = initialButton;
            if(initialButton) currentButton.SelectButton();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            throw new NotImplementedException();
        }

        private void SetButton(ButtonNavigation newButton)
        {
            if (!newButton) return;
            currentButton.DeselectButton();
            currentButton = newButton;
            currentButton.SelectButton();
        }

        private void MoveLeft()
        {
            SetButton(currentButton.Left);
        }

        private void MoveRight()
        {
            SetButton(currentButton.Right);
        }

        private void MoveUp()
        {
            SetButton(currentButton.Up);
        }

        private void MoveDown()
        {
            SetButton(currentButton.Down);
        }

        private void PressButton()
        {
            currentButton.PressButton();
        }
    }
}