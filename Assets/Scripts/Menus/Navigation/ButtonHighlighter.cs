using System;
using UnityEngine;

namespace Menus.Navigation
{
    public class ButtonHighlighter : MonoBehaviour
    {
        [SerializeField] private ButtonNavigation buttonNavigation;

        private void OnEnable()
        {
            buttonNavigation.onSelect.AddListener(EnableHighlight);
            buttonNavigation.onDeselect.AddListener(DisableHighlight);
        }

        private void OnDisable()
        {
            buttonNavigation.onSelect.RemoveListener(EnableHighlight);
            buttonNavigation.onDeselect.RemoveListener(DisableHighlight);
        }

        private void EnableHighlight()
        {
            
        }

        private void DisableHighlight()
        {
            
        }
    }
}