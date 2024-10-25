using UnityEngine;
using UnityEngine.Events;

namespace Menus.Navigation
{
    public class ButtonNavigation : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button button;
        public ButtonNavigation Left;
        public ButtonNavigation Right;
        public ButtonNavigation Up;
        public ButtonNavigation Down;

        public UnityEvent onSelect;
        public UnityEvent onDeselect;

        public void SelectButton()
        {
            onSelect.Invoke();
        }
        
        public void DeselectButton()
        {
            onDeselect.Invoke();
        }

        public void PressButton()
        {
            button.onClick.Invoke();
        }
    }
}