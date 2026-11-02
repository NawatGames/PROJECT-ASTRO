using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menus.Pause
{
    // Overlay CRT da pausa: acompanha o retângulo da tela e anima o tubo ligando ao abrir e desligando ao fechar.
    // Usa tempo não escalado porque a pausa roda com Time.timeScale = 0.
    [RequireComponent(typeof(Image))]
    public class CrtScreen : MonoBehaviour
    {
        private static readonly int PowerOnId = Shader.PropertyToID("_PowerOn");
        private static readonly int UnscaledTimeId = Shader.PropertyToID("_UnscaledTime");

        [SerializeField] private RectTransform screenRect;
        [SerializeField] private float powerOnDuration = 0.35f;
        [SerializeField] private float powerOffDuration = 0.25f;

        private Material _material;
        private float _animationStartTime;
        private bool _isPoweringOff;
        private Action _onPoweredOff;

        private void Awake()
        {
            var image = GetComponent<Image>();
            _material = new Material(image.material);
            image.material = _material;
        }

        private void OnEnable()
        {
            MatchScreenRect();
            _isPoweringOff = false;
            _onPoweredOff = null;
            _animationStartTime = Time.unscaledTime;
            _material.SetFloat(PowerOnId, 0f);
        }

        // Fecha o tubo (a tela vira uma linha e depois um ponto) e chama onComplete quando apagar.
        public void PowerOff(Action onComplete)
        {
            _isPoweringOff = true;
            _onPoweredOff = onComplete;
            _animationStartTime = Time.unscaledTime;
        }

        private void Update()
        {
            float duration = _isPoweringOff ? powerOffDuration : powerOnDuration;
            float progress = duration > 0f ? Mathf.Clamp01((Time.unscaledTime - _animationStartTime) / duration) : 1f;

            _material.SetFloat(PowerOnId, _isPoweringOff ? 1f - progress : progress);
            _material.SetFloat(UnscaledTimeId, Time.unscaledTime);

            if (_isPoweringOff && progress >= 1f && _onPoweredOff != null)
            {
                var onPoweredOff = _onPoweredOff;
                _onPoweredOff = null;
                onPoweredOff();
            }
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
