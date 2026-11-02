using UnityEngine;
using UnityEngine.UI;

namespace Menus.Pause
{
    // Overlay CRT da pausa: acompanha o retângulo da tela e anima o "ligar o tubo" ao abrir.
    // Usa tempo não escalado porque a pausa roda com Time.timeScale = 0.
    [RequireComponent(typeof(Image))]
    public class CrtScreen : MonoBehaviour
    {
        private static readonly int PowerOnId = Shader.PropertyToID("_PowerOn");
        private static readonly int UnscaledTimeId = Shader.PropertyToID("_UnscaledTime");

        [SerializeField] private RectTransform screenRect;
        [SerializeField] private float powerOnDuration = 0.35f;

        private Material _material;
        private float _powerOnStartTime;

        private void Awake()
        {
            var image = GetComponent<Image>();
            _material = new Material(image.material);
            image.material = _material;
        }

        private void OnEnable()
        {
            MatchScreenRect();
            _powerOnStartTime = Time.unscaledTime;
            _material.SetFloat(PowerOnId, 0f);
        }

        private void Update()
        {
            float progress = powerOnDuration > 0f ? (Time.unscaledTime - _powerOnStartTime) / powerOnDuration : 1f;
            _material.SetFloat(PowerOnId, Mathf.Clamp01(progress));
            _material.SetFloat(UnscaledTimeId, Time.unscaledTime);
        }

        private void OnDestroy()
        {
            Destroy(_material);
        }

        // A cena sobrescreve o tamanho da tela, então o overlay copia o retângulo dela em vez de ter um fixo.
        private void MatchScreenRect()
        {
            if (!screenRect) return;

            var rect = (RectTransform)transform;
            rect.anchorMin = screenRect.anchorMin;
            rect.anchorMax = screenRect.anchorMax;
            rect.pivot = screenRect.pivot;
            rect.anchoredPosition = screenRect.anchoredPosition;
            rect.sizeDelta = screenRect.sizeDelta;
        }
    }
}
