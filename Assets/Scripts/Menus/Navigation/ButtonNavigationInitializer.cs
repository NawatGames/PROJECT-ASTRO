using System;
using UnityEngine;

namespace Menus.Navigation
{
    public class ButtonNavigationInitializer : MonoBehaviour
    {
        [SerializeField] private NavigationManager navigationManager;
        [SerializeField] private ButtonNavigation buttonNavigation;

        private void OnEnable()
        {
            navigationManager.OverrideCurrentButton(buttonNavigation);
        }
    }
}