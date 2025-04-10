using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndingVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string menuSceneName = "MenuScene";

    void Start()
    {
        // Ensure the VideoPlayer is attached
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                Debug.LogError("No VideoPlayer found! Please attach a VideoPlayer component.");
                return;
            }
        }

        // Ensure the video is set and ready to play
        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.prepareCompleted += PrepareCompleted;
            videoPlayer.Prepare();
        }

        // Register the callback for when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    // Called once video has finished playing
    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished! Loading Menu scene.");
        SceneManager.LoadScene(menuSceneName);
    }

    // Called when video preparation completes
    private void PrepareCompleted(VideoPlayer vp)
    {
        videoPlayer.Play();
    }
}
