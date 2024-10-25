using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.Navigation
{
    public class ButtonHighlighter : MonoBehaviour
    {
        [SerializeField] private Color selectedColor = new Color(255,0,0);
        
        [SerializeField] private Image buttonImage;
        [SerializeField] private ButtonNavigation buttonNavigation;

        private Color _initialColor;

        private void Awake()
        {
            _initialColor = buttonImage.color;
        }

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
            buttonImage.color = selectedColor;
        }

        private void DisableHighlight()
        {
            buttonImage.color = _initialColor;
        }
    }
}