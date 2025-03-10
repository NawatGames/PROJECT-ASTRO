using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Menus.Navigation
{
    public class NavigationManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        [SerializeField] private ButtonNavigation initialButton;
        private ButtonNavigation _currentButton;

        private MenuNavigation _inputAsset;

        private void Awake()
        {
            if(initialButton) SetButton(initialButton);
            
            _inputAsset = new MenuNavigation();
            input.actions = _inputAsset.asset;
        }

        private void OnEnable()
        {
            _inputAsset.Default.Up.performed += MoveUp;
            _inputAsset.Default.Down.performed += MoveDown;
            _inputAsset.Default.Left.performed += MoveLeft;
            _inputAsset.Default.Right.performed += MoveRight;
            _inputAsset.Default.Enter.performed += PressButton;
        }

        private void OnDisable()
        {
            _inputAsset.Default.Up.performed -= MoveUp;
            _inputAsset.Default.Down.performed -= MoveDown;
            _inputAsset.Default.Left.performed -= MoveLeft;
            _inputAsset.Default.Right.performed -= MoveRight;
            _inputAsset.Default.Enter.performed -= PressButton;
        }

        private void SetButton(ButtonNavigation newButton)
        {
            if (!newButton) return;
            if(_currentButton) _currentButton.DeselectButton();
            _currentButton = newButton;
            _currentButton.SelectButton();
        }

        private void MoveLeft(InputAction.CallbackContext ctx)
        {
            SetButton(_currentButton.Left);
        }

        private void MoveRight(InputAction.CallbackContext ctx)
        {
            SetButton(_currentButton.Right);
        }

        private void MoveUp(InputAction.CallbackContext ctx)
        {
            SetButton(_currentButton.Up);
        }

        private void MoveDown(InputAction.CallbackContext ctx)
        {
            SetButton(_currentButton.Down);
        }

        private void PressButton(InputAction.CallbackContext ctx)
        {
            _currentButton.PressButton();
        }
    }
}