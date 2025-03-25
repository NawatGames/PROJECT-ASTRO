using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Tasks.MemoryTask
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MemoryTile : MonoBehaviour
    {
        [SerializeField] private MemoryTask memoryTask;
        [SerializeField] Color offColor = Color.gray;
        [SerializeField] private Light2D light;

        private SpriteRenderer _spriteRenderer;
        private Color _currentColor;
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.color = offColor;
            _currentColor = offColor;
        }

        private void OnEnable()
        {
            memoryTask.onTileDisable.AddListener(OnTileDisable);
        }

        private void OnDisable()
        {
            memoryTask.onTileDisable.RemoveListener(OnTileDisable);
        }

        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
            light.color = color;
            light.enabled = true;
            _currentColor = color;
        }

        private void OnTileDisable()
        {
            _spriteRenderer.color = offColor;
            light.enabled = false;
            _currentColor = offColor;
        }

        public Color GetCurrentColor()
        {
            return _currentColor;
        }
    }
}
