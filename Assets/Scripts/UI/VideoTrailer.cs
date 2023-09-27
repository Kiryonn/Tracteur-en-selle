using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoTrailer : MonoBehaviour
{
    [SerializeField] CanvasGroup video;
    [SerializeField] float timeUntilVideo;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Credits credits;
    [SerializeField] LoadingScreenTips loadingScreen;

    [SerializeField] float fadeInSpeed;
    [SerializeField] float fadeOutSpeed;

    bool videoIsPlaying;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        video.gameObject.SetActive(false);
        video.alpha = 0f;
    }

    private void Update()
    {
        if (Input.anyKeyDown || credits.isStarted || loadingScreen.loading)
        {
            timer = 0f;
            if (videoIsPlaying)
            {
                HideVideo();
            }
        }

        if (!videoIsPlaying)
        {
            timer += Time.deltaTime;
            if (timer > timeUntilVideo)
            {
                timer = 0f;
                ShowVideo();
            }
        }
        else
        {
            timer = 0f;
        }
        
    }

    IEnumerator FadeVideo(float speed, float targetAlpha)
    {
        float startAlpha = video.alpha;
        video.gameObject.SetActive(true);

        for (float i = 0f; i < 1.0f; i += Time.deltaTime / speed)
        {
            video.alpha = Mathf.Lerp(startAlpha, targetAlpha, i);
            yield return new WaitForEndOfFrame();
        }

        video.alpha = targetAlpha;

        bool target = targetAlpha > 0f;

        videoIsPlaying = target;
        video.gameObject.SetActive(target);
    }

    public void ShowVideo()
    {
        StartCoroutine(FadeVideo(fadeInSpeed, 1f));
    }

    public void HideVideo()
    {
        StartCoroutine(FadeVideo(fadeOutSpeed, 0f));
    }
}
