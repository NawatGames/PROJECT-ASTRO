using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks.MemoryTask
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MemoryTile : MonoBehaviour
    {
        [SerializeField] private MemoryTask memoryTask;
        [SerializeField] Color offColor = Color.gray;

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
            _currentColor = color;
        }

        private void OnTileDisable()
        {
            _spriteRenderer.color = offColor;
            _currentColor = offColor;
        }

        public Color GetCurrentColor()
        {
            return _currentColor;
        }
    }
}
