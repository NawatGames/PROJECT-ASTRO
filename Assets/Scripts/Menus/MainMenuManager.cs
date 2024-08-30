namespace Menus
{
    using UnityEngine;

    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject mainSettingsPanel;
        [SerializeField] private GameObject gameSettingsPanel;
        [SerializeField] private GameObject audioSettingsPanel;
        [SerializeField] private GameObject keyboardSettingsPanel;
        [SerializeField] private GameObject restoreDefaultSettingsPanel;
        private GameObject _currentPanel;
        private GameObject _previousPanel;

        private void Awake()
        {
            _currentPanel = mainMenuPanel;
            _previousPanel = null;
        }

        public void ActivatePanel(GameObject panelToBeActivated)
        {
            panelToBeActivated.SetActive(true);
            _previousPanel = _currentPanel;
            _currentPanel = panelToBeActivated;
            _previousPanel.SetActive(false);
        }
    }

}