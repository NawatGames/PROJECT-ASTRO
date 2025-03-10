using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer video;
    [SerializeField] private RawImage screen;
    public bool isPlaying = true;

    // Start is called before the first frame update
    void Start()
    {
        if (video != null)
        {
            video.loopPointReached += OnVideoEnd;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isPlaying = video.isPlaying;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        screen.gameObject.SetActive(false);
        SceneManager.LoadScene("StartMenuAnimations");
    }
}
