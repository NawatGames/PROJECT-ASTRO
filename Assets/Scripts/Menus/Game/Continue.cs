using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus.Game
{
    public class Continue : MonoBehaviour
    { 
        private void Start()
        {
            if (SaveManager.CurrentLevel > 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public void ContinueGame()
        {
            SceneManager.LoadScene("Scene Build");
        }
    }
}