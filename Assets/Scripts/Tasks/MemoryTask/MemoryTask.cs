using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Tasks.MemoryTask
{
    public class MemoryTask : TaskScript
    {
        [Header("Task Setup")]
        [SerializeField] private List<Color> easyModeColors;
        [SerializeField] private List<Color> hardModeColors;
        [SerializeField] private GameObject tilesHolder;

        [Header("Task Config")]
        //[SerializeField] private int roundCount = 1;
        [SerializeField] private float memorizationTime = 2f;
        [SerializeField] private float inputTime = 2f;
        
        public UnityEvent onTileDisable;
        private List<MemoryTile> _tiles;

        private int _buttonPressedIndex = -1;

        private List<Color> _colors;
        private Color _correctColor;
        private bool _wasSelected = false;

        protected override void Awake()
        {
            base.Awake();
            _tiles = tilesHolder.GetComponentsInChildren<MemoryTile>().ToList();
            // Checks if there are enough colors for task to work properly
            if (easyModeColors.Count < _tiles.Count * 2)
            {
                throw new System.Exception("Not enough colors set for easy mode");
            }
            if (_tiles.Count >= hardModeColors.Count * 2)
            {
                throw new System.Exception("Not enough colors set for hard mode");
            }
        }

        protected override void RunTask()
        {
            base.RunTask();

            if (isAstroSpecialist == isAstro)
            {
                _colors = easyModeColors;
            }
            else
            {
                _colors = hardModeColors;
            }
            RoundSetup();
        }

        protected override void TaskMistakeLeave()
        {
            base.TaskMistakeLeave();
            onTileDisable.Invoke();
        }

        protected override void TaskSuccessful()
        {
            base.TaskSuccessful();
            onTileDisable.Invoke();
        }

        public override void EndTask()
        {
            base.EndTask();
            _wasSelected = false;
            _buttonPressedIndex = 0;
            onTileDisable.Invoke();
        }

        private void RoundSetup()
        {
            // Shuffle colors
            _colors = _colors.OrderBy(x => Random.value).ToList();

            List<Color> initialColors = new();
            // Assign colors to tiles
            for (int i = 0; i < _tiles.Count; i++)
            {
                // Get color from list
                Color randomColor = _colors[i];
                // Store colors initially displayed on tiles
                initialColors.Add(randomColor);
                _tiles[i].SetColor(randomColor);
                // Remove color from original list
                _colors.Remove(randomColor);
            }

            // Choose correct color to be memorized
            _correctColor = initialColors[Random.Range(0, initialColors.Count)];

            StartCoroutine(Round());
        }

        private IEnumerator Round()
        {
            // Memorization Time
            yield return new WaitForSeconds(memorizationTime);
            onTileDisable.Invoke();
            yield return new WaitForSeconds(2f);

            // Assign colors to tiles
            for (int i = 0; i < _tiles.Count; i++)
            {
                _tiles[i].SetColor(_colors[i]);
            }
            int randomIndex = Random.Range(0, _tiles.Count);
            _tiles[randomIndex].SetColor(_correctColor);
            _wasSelected = true;

            // Input Time
            yield return new WaitForSeconds(inputTime);
        }

        private void VerifyPoint()
        {
            Color selectedColor = _tiles[_buttonPressedIndex].GetCurrentColor();
            for (int i = 0; i < _tiles.Count; i++)
            {
                if (i != _buttonPressedIndex)
                {
                    _tiles[i].GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
            if (selectedColor == _correctColor)
            {
                TaskSuccessful();
            }
            else
            {
                TaskMistakeLeave();
            }
        }

        // Tile Selection
        protected override void OnUpPerformed(InputAction.CallbackContext value)
        {
            UpdateSelection(0);
        }

        protected override void OnDownPerformed(InputAction.CallbackContext value)
        {
            UpdateSelection(2);
        }

        protected override void OnLeftPerformed(InputAction.CallbackContext value)
        {
            UpdateSelection(1);
        }

        protected override void OnRightPerformed(InputAction.CallbackContext value)
        {
            UpdateSelection(3);
        }
        
        private void UpdateSelection(int choice)
        {
            if (_wasSelected)
            {
                _buttonPressedIndex = choice;
                VerifyPoint();
            }
        }
    }
}