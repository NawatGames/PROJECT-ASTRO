using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus.Game
{
    public class NewGame : MonoBehaviour
    {
        public void StartNewGame()
        {
            SaveManager.CreateNewSaveFile();
            SceneManager.LoadScene("TechFinal");
        }
    }
}