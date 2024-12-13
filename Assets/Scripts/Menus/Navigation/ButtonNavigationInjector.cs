using System;
using System.Collections.Generic;
using UnityEngine;

namespace Menus.Navigation
{
    [Serializable]
    public struct InjectedNavigation
    {
        public ButtonNavigation buttonNavigation;
        public NavigationDirections newNavigationDirections;
    }
    /// <summary>
    /// Updates a menu navigation path when the button is enabled or disabled
    /// </summary>
    public class ButtonNavigationInjector : MonoBehaviour
    {
        [SerializeField] private List<InjectedNavigation> _injectedNavigations;
        private List<InjectedNavigation> _initialNavigation;
        private void Awake()
        {
            _initialNavigation = new();
            foreach (InjectedNavigation nav in _injectedNavigations)
            {
                InjectedNavigation initialNav = new();
                initialNav.buttonNavigation = nav.buttonNavigation;
                initialNav.newNavigationDirections = nav.buttonNavigation.directions;
                _initialNavigation.Add(initialNav);
            }
        }

        private void OnEnable()
        {
            foreach (InjectedNavigation nav in _injectedNavigations)
            {
                nav.buttonNavigation.directions = nav.newNavigationDirections;
            }
        }

        private void OnDisable()
        {
            foreach (InjectedNavigation nav in _initialNavigation)
            {
                nav.buttonNavigation.directions = nav.newNavigationDirections;
            }
        }
    }
}