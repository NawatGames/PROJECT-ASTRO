using TMPro;
using UnityEngine;

namespace Menus.Pause
{
    // Cursor piscando no fim de um texto de terminal. O sufixo fica transparente em vez de sumir,
    // assim o texto não muda de largura a cada piscada.
    [RequireComponent(typeof(TMP_Text))]
    public class CrtBlinkingSuffix : MonoBehaviour
    {
        [SerializeField] private string suffix = "_";
        [SerializeField] private float interval = 0.5f;

        private TMP_Text _text;
        private string _baseText;
        private bool _visible;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _baseText = _text.text;
        }

        private void OnEnable()
        {
            Refresh(true);
        }

        private void Update()
        {
            bool visible = Mathf.FloorToInt(Time.unscaledTime / interval) % 2 == 0;
            if (visible != _visible) Refresh(visible);
        }

        private void Refresh(bool visible)
        {
            _visible = visible;
            _text.text = visible ? _baseText + suffix : $"{_baseText}<alpha=#00>{suffix}";
        }
    }
}
