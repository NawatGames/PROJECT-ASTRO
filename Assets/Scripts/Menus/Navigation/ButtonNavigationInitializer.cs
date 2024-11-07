using System;
using UnityEngine;

namespace Menus.Navigation
{
    /// <summary>
    /// Sets NavigationManager initial button when the menu is loaded
    /// </summary>
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