using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menus.Pause
{
    // Painel principal da pausa: seleciona o primeiro item ao abrir e nunca deixa o menu sem seleção,
    // pra o cursor não sumir quando o jogador clica fora dos botões.
    public class CrtMenu : MonoBehaviour
    {
        [SerializeField] private Selectable firstSelected;
        [Tooltip("Objetos que só aparecem junto com o menu principal (cabeçalho e rodapé da tela).")]
        [SerializeField] private GameObject[] showWithMenu;

        private GameObject _lastSelected;

        private void OnEnable()
        {
            SetExtrasActive(true);

            var eventSystem = EventSystem.current;
            if (!eventSystem || !firstSelected) return;

            // Limpa antes pra o OnSelect disparar mesmo se o botão já era o selecionado da última pausa.
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(firstSelected.gameObject);
            _lastSelected = firstSelected.gameObject;
        }

        private void OnDisable()
        {
            SetExtrasActive(false);
        }

        private void Update()
        {
            var eventSystem = EventSystem.current;
            if (!eventSystem) return;

            var current = eventSystem.currentSelectedGameObject;
            if (current && current.transform.IsChildOf(transform))
            {
                _lastSelected = current;
            }
            else if (!current && _lastSelected)
            {
                eventSystem.SetSelectedGameObject(_lastSelected);
            }
        }

        private void SetExtrasActive(bool active)
        {
            foreach (var extra in showWithMenu)
            {
                if (extra) extra.SetActive(active);
            }
        }
    }
}
