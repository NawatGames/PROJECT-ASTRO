using Audio_System;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SceneVideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string targetSceneName;
    [SerializeField] private VideoClip trailerVideo;
    [SerializeField] private VideoClip startVideo;
    [SerializeField] private VideoClip loopVideo;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private AudioPlayer audioPlayer;

    private bool isSceneActive = false;
    private int videoPhase = 0;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        SetButtonsActive(false);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == targetSceneName)
        {
            if (!isSceneActive)
            {
                StartVideo();
                isSceneActive = true;
            }

            if (videoPhase == 0 && Input.anyKeyDown)
            {
                SkipToNextVideo();
            }
        }
        else
        {
            isSceneActive = false;
            videoPlayer.Stop();
        }
    }

    void StartVideo()
    {
        videoPlayer.clip = trailerVideo;
        videoPlayer.isLooping = false;
        videoPlayer.time = 0.4f;
        videoPlayer.Play();

        SetButtonsActive(false);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (videoPhase == 0)
        {
            PlayStartVideo();
        }
        else if (videoPhase == 1)
        {
            PlayLoopVideo();
        }
    }

    void SkipToNextVideo()
    {
        if (videoPhase == 0)
        {
            PlayStartVideo();
        }
    }

    void PlayStartVideo()
    {
        videoPlayer.clip = startVideo;
        videoPlayer.isLooping = false;
        videoPlayer.Play();
        videoPlayer.SetDirectAudioVolume(0,.4f);
        audioPlayer.PlayAudio();
        videoPhase = 1;
    }

    void PlayLoopVideo()
    {
        videoPlayer.clip = loopVideo;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
        videoPhase = 2;

        SetButtonsActive(true);
    }

    void SetButtonsActive(bool isActive)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(isActive);
        }
    }
}