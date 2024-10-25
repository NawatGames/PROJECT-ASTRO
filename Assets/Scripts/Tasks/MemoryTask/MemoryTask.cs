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
        [SerializeField] private GameObject tilesSelector;

        [Header("Task Config")] 
        [SerializeField] private int gridSize = 2;
        //[SerializeField] private int roundCount = 1;
        [SerializeField] private float memorizationTime = 2f;
        [SerializeField] private float inputTime = 2f;
        
        private Vector2Int _selectorPosition = new();
        public UnityEvent onTileDisable;
        private List<MemoryTile> _tiles;
        
        private List<Color> _colors;
        private Color _correctColor;
        
        protected override void Awake()
        {
            base.Awake();
            tilesSelector.SetActive(false);
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
            _colors = easyModeColors;
            // TODO: Choose difficulty
            //if ()
            //{
            //    _colors = easyModeColors;
            //}
            //else
            //{
            //    _colors = hardModeColors;
            //}
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
            tilesSelector.SetActive(false);
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
            tilesSelector.SetActive(true);
            
            // Input Time
            yield return new WaitForSeconds(inputTime);
            int selectedTileIndex = _selectorPosition.y * gridSize + _selectorPosition.x;
            Color selectedColor = _tiles[selectedTileIndex].GetCurrentColor();
            if (selectedColor == _correctColor)
            {
                TaskSuccessful();
            }
            else
            {
                TaskMistakeLeave();
            }
            tilesSelector.SetActive(false);
        }

        // Tile Selection
        protected override void OnUpPerformed(InputAction.CallbackContext value)
        {
            if (_selectorPosition.y > 0)
            {
                _selectorPosition.y--;
                UpdateSelection();
            }
        }
        
        protected override void OnDownPerformed(InputAction.CallbackContext value)
        {
            if (_selectorPosition.y < gridSize - 1)
            {
                _selectorPosition.y++;
                UpdateSelection();
            }
        }
        
        protected override void OnLeftPerformed(InputAction.CallbackContext value)
        {
            if (_selectorPosition.x > 0)
            {
                _selectorPosition.x--;
                UpdateSelection();
            }
        }
        
        protected override void OnRightPerformed(InputAction.CallbackContext value)
        {
            if (_selectorPosition.x < gridSize - 1)
            {
                _selectorPosition.x++;
                UpdateSelection();
            }
        }

        // Sets selector position to the selected tile
        private void UpdateSelection()
        {
            if(!tilesSelector.activeSelf) return;
            int index = _selectorPosition.y * gridSize + _selectorPosition.x;
            Vector3 position = _tiles[index].transform.position;
            tilesSelector.transform.position = position;
        }
    }
}