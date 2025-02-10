using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipCutsceneHandler : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text skipText;
    private float fillAmount = 0.5f;
    private bool isPressed = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Submit") > 0)
        {
            isPressed = true;
            skipText.gameObject.SetActive(true);
        }
        if (Input.GetAxisRaw("Submit") == 0)
        {
            isPressed = false;
            skipText.gameObject.SetActive(false);
            progressBar.fillAmount = 0;
        }
        if (isPressed)
        {
            progressBar.fillAmount += fillAmount/40;
        }
        if(progressBar.fillAmount == 1)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}
