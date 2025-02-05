using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class GameOverVideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private VideoClip jumpscareVideo;
    [SerializeField] private RenderTexture renderTexture;

    private void Awake()
    {
        if (rawImage != null)
        {
            rawImage.color = new Color(1, 1, 1, 0);
            rawImage.texture = renderTexture;
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        ResetRenderTexture();
    }

    public void StartJumpscareVideo()
    {
        ResetRenderTexture();

        if (rawImage != null)
        {
            rawImage.texture = renderTexture;
            rawImage.color = new Color(1, 1, 1, 1);
        }

        videoPlayer.clip = jumpscareVideo;
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }


    public void StopVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        ResetRenderTexture();

        if (rawImage != null)
        {
            rawImage.color = new Color(1, 1, 1, 0);
        }
    }

    private void ResetRenderTexture()
    {
        if (renderTexture != null)
        {
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
        }
    }


    public VideoPlayer getVideoPlayer()
    {
        return videoPlayer;
    }

    public RawImage GetRawImage()
    {
        return rawImage;
    }

    public VideoPlayer GetVideoPlayer()
    {
        return videoPlayer;
    }

}
