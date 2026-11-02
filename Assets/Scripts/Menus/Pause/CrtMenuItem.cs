using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menus.Pause
{
    // Item do menu CRT: o selecionado ganha o cursor ">" e brilho total, os outros ficam apagados.
    // Mouse e teclado passam pela mesma seleção do EventSystem.
    [RequireComponent(typeof(Button))]
    public class CrtMenuItem : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private GameObject cursor;
        [SerializeField, Range(0f, 1f)] private float deselectedAlpha = 0.5f;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            var eventSystem = EventSystem.current;
            SetHighlighted(eventSystem && eventSystem.currentSelectedGameObject == gameObject);
        }

        public void OnSelect(BaseEventData eventData)
        {
            SetHighlighted(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            SetHighlighted(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_button.interactable) _button.Select();
        }

        private void SetHighlighted(bool highlighted)
        {
            cursor.SetActive(highlighted);
            label.alpha = highlighted ? 1f : deselectedAlpha;
        }
    }
}
