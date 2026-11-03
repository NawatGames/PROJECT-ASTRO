using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menus
{
    // Efeito de hover dos botões do menu: o botão cresce, o texto esquenta de cor e uma faixa aparece atrás.
    [RequireComponent(typeof(Button))]
    public class MenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Color hoverTextColor = new Color(1f, 0.6f, 0.3f, 1f);
        [SerializeField] private Color hoverBackgroundColor = new Color(1f, 1f, 1f, 0.1f);
        [SerializeField] private float hoverScale = 1.08f;
        [SerializeField] private float pressedScale = 0.97f;
        [SerializeField] private float speed = 14f;

        private Button _button;
        private Image _background;
        private TMP_Text _label;
        private Color _normalTextColor;
        private Color _normalBackgroundColor;
        private bool _isHovered;
        private bool _isPressed;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _background = GetComponent<Image>();
            _label = GetComponentInChildren<TMP_Text>(true);
            _normalTextColor = _label.color;
            if (_background) _normalBackgroundColor = _background.color;
        }

        // O botão pode sumir com o mouse em cima (troca de painel), e o evento de saída nunca chegaria.
        private void OnDisable()
        {
            _isHovered = false;
            _isPressed = false;
            // Ao trocar de cena o texto pode já ter sido destruído.
            if (_label) Apply(1f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = _button.interactable;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = _button.interactable;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
        }

        private void Update()
        {
            Apply(1f - Mathf.Exp(-speed * Time.unscaledDeltaTime));
        }

        private void Apply(float blend)
        {
            float targetScale = _isPressed ? pressedScale : _isHovered ? hoverScale : 1f;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * targetScale, blend);
            _label.color = Color.Lerp(_label.color, _isHovered ? hoverTextColor : _normalTextColor, blend);
            if (_background)
            {
                _background.color = Color.Lerp(_background.color, _isHovered ? hoverBackgroundColor : _normalBackgroundColor, blend);
            }
        }
    }
}
