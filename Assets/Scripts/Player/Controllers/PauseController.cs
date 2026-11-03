using Menus.Pause;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject view;

    private bool _isPaused = false;
    private bool _isClosing = false;
    private CrtScreen _crtScreen;

    void Awake()
    {
        // Se a tela de pausa tiver o overlay CRT, a pausa só fecha depois da animação de desligar o tubo.
        _crtScreen = view.GetComponentInChildren<CrtScreen>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggledHandler();
        }
    }

    public void PauseToggledHandler()
    {
        // Ignora ESC e cliques enquanto o tubo está desligando, senão a pausa reabriria no meio da animação.
        if (_isClosing) return;

        _isPaused = !_isPaused;
        if (_isPaused) // Pausou
        {
            view.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (_crtScreen) // Despausou, esperando o tubo desligar
        {
            _isClosing = true;
            _crtScreen.PowerOff(Close);
        }
        else // Despausou
        {
            Close();
        }
    }

    private void Close()
    {
        _isClosing = false;
        view.SetActive(false);
        Time.timeScale = 1f;
    }
}
