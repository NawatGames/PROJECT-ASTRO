using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject view;

    private bool _isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggledHandler();
        }
    }

    public void PauseToggledHandler()
    {
        _isPaused = !_isPaused;
        if (_isPaused) // Pausou
        {
            view.SetActive(true);
            Time.timeScale = 0f;
        }
        else // Despausou
        {
            view.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
