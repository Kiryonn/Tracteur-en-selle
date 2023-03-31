using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenTips : MonoBehaviour
{
    [SerializeField] Image[] backgroundImages;
    int currentImageIndex;
    [SerializeField] float durationPerImage;
    float currentTimer;
    [SerializeField] float fadeSpeed;
    bool loading;
    [SerializeField] Canvas loadingCanvas;
    public Image loadgingBarFill;

    public static LoadingScreenTips instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        loadingCanvas.gameObject.SetActive(false);
        currentImageIndex = Random.Range(0, backgroundImages.Length);
        SetAllOpacity();
    }

    public void StartLoading()
    {
        loadingCanvas.gameObject.SetActive(true);
        Color c = backgroundImages[currentImageIndex].color;
        c.a = 1f;
        backgroundImages[currentImageIndex].color = c;
        //StartCoroutine(FadeImage(backgroundImages[currentImageIndex]));
        loading = true;
    }

    private void Update()
    {
        if (loading)
        {
            if (currentTimer < durationPerImage)
            {
                currentTimer += Time.deltaTime;
            }
            else
            {
                currentTimer = 0f;
                StartCoroutine(CrossFadeImage(backgroundImages[currentImageIndex], SelectImage()));
            }
        }
        
    }

    IEnumerator FadeImage(Image img)
    {
        Color c = img.color;
        c.a = 0f;

        for (float i = 0f; i < 1f; i += Time.deltaTime / fadeSpeed)
        {
            c.a = Mathf.Lerp(0f, 1f, i);

            img.color = c;

            yield return null;
        }
        c.a = 1f;
        img.color = c;
        currentTimer = 0f;
    }

    IEnumerator CrossFadeImage(Image current, Image final)
    {
        Color noAlpha = current.color;
        noAlpha.a = 1f;

        Color alpha = current.color;
        alpha.a = 0f;

        for (float i = 0f; i < 1f; i+=Time.deltaTime/fadeSpeed)
        {
            alpha.a = Mathf.Lerp(0f, 1f, i);
            noAlpha.a = Mathf.Lerp(1f, 0f, i);

            current.color = noAlpha;
            final.color = alpha;

            yield return null;
        }
        noAlpha.a = 0f;
        alpha.a = 1f;

        current.color = noAlpha;
        final.color = alpha;
        currentTimer = 0f;
    }

    void SetAllOpacity()
    {
        Color a = backgroundImages[0].color;
        a.a = 0f;
        for (int i = 0; i < backgroundImages.Length; i++)
        {
            backgroundImages[i].color = a;
        }
    }

    Image SelectImage()
    {
        int r = Random.Range(0, backgroundImages.Length);
        while(r == currentImageIndex)
        {
            r = Random.Range(0, backgroundImages.Length);
        }
        currentImageIndex = r;
        return backgroundImages[r];
    }
}
