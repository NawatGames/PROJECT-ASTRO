using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus.Game
{
    public class Continue : MonoBehaviour
    {
        [Tooltip("Botão que só aparece quando há save. Vazio usa o primeiro filho, como nas cenas antigas.")]
        [SerializeField] private GameObject continueButton;

        private void Start()
        {
            if (SaveManager.CurrentLevel > 0)
            {
                var button = continueButton ? continueButton : transform.GetChild(0).gameObject;
                button.SetActive(true);
            }
        }

        public void ContinueGame()
        {
            SceneManager.LoadScene(SceneNames.Game);
        }
    }
}