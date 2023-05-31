using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    Camera cam;
    public Light globalLight;
    public Light spot;
    public Transform tractorPosition;
    public GameObject oldUI;

    [Header("Fade UI")]

    public Image fadeScreenImage;
    public Image fadeDamageImage;
    public float fadeDuration;

    [Header("Delays")]

    public DelaysData delays;

    [Header("UI Elements Score")]

    public UIDataText tempsText;
    public UIDataText echecText;
    public UIDataText duraText;

    public UIDataText totalText;
    public UIDataText pointNumberText;
    public UIDataText ptsText;

    [Header("UI Elements Medical")]

    public UIDataImage schemaHumain;

    float score;
    int index = 0;

    [System.Serializable]
    public class UIDataText
    {
        public TextMeshProUGUI txt;
        public Vector3 position { get; private set; }
        public Color color { get; private set; }

        public virtual void Init()
        {
            Color col = txt.color;
            col.a = 0f;
            position = txt.rectTransform.position;
            color = col;
            txt.color = col;
            txt.gameObject.SetActive(true);
        }

        public virtual void Reset()
        {
            txt.color = color;
            txt.rectTransform.position = position;
            Color col = txt.color;
            col.a = 0f;
            txt.color = col;
        }

        public virtual void Show()
        {
            Color col = txt.color;
            col.a = 1f;
            txt.color = col;
            txt.gameObject.SetActive(true);
        }
    }

    [System.Serializable]
    public class UIDataImage
    {
        public Image img;
        public Vector3 position { get; private set; }
        public Color color { get; private set; }

        public virtual void Init()
        {
            Color col = img.color;
            col.a = 0f;
            position = img.rectTransform.position;
            color = col;
            img.color = col;
            img.gameObject.SetActive(true);
        }

        public virtual void Reset()
        {
            img.rectTransform.position = position;
            img.color = color;
            Color col = img.color;
            col.a = 0f;
            img.color = col;
        }

        public virtual void Show()
        {
            Color col = img.color;
            col.a = 1f;
            img.color = col;
            img.gameObject.SetActive(true);
        }
    }
        


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        InitUIData();

        // Fade screen from black to white
        StartCoroutine(FadeScreen(0f, fadeDuration, false));
    }

    public void SetValues(float t, float e, float s, float scr)
    {
        score = scr;
        tempsText.txt.text = "temps : " + (int)t + " secondes";
        echecText.txt.text = "echecs : " + e;
        duraText.txt.text = "sante du tracteur : " + s + "%";
        FadeToBlack();
    }

    IEnumerator FadeScreen(float endAlpha, float duration, bool hideGameobject)
    {
        float a = fadeScreenImage.color.a;
        Color c = fadeScreenImage.color;

        if (!hideGameobject)
        {
            fadeScreenImage.gameObject.SetActive(true);
        }
        else
        {
            fadeScreenImage.gameObject.SetActive(false);
        }

        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / duration)
        {
            c.a = Mathf.Lerp(a, endAlpha, i);
            fadeScreenImage.color = c;
            yield return null;
        }
        c.a = endAlpha;
        fadeScreenImage.color = c;
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeScreen(1f, fadeDuration, false));
        Invoke("RenderBlack", fadeDuration + 2f);
    }

    public void RenderBlack()
    {
        Debug.Log("We are rendering black");
        bool check = !SettingsManager.instance.isGarageOrTutorial();
        SettingsManager.instance.LoadNextLevel();
        // Making the scene black

        cam.enabled = false;
        RenderSettings.reflectionIntensity = 0f;
        RenderSettings.ambientLight = Color.black;
        RenderSettings.ambientIntensity = 0f;
        globalLight.enabled = false;
        spot.gameObject.SetActive(true);

        // Setting the player position and rotation

        if (check) { MovePlayerToScorePosition(); }

        // Removing the black foreground
        oldUI.SetActive(false);
        fadeScreenImage.gameObject.SetActive(false);

        if (check) { ShowScoreParam(); } else { RenderWhite(); }
        
    }

    void MovePlayerToScorePosition()
    {
        GameManager.Instance.player.canMove = false;
        GameObject player = GameManager.Instance.velo.gameObject;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.transform.SetParent(tractorPosition);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
        player.transform.localScale = Vector3.one;
    }

    public void RenderWhite()
    {
        SettingsManager.instance.LoadMainMenu();
        cam.enabled = true;
        RenderSettings.reflectionIntensity = 1f;
        RenderSettings.ambientLight = Color.white;
        RenderSettings.ambientIntensity = 1f;
        globalLight.enabled = true;
        spot.gameObject.SetActive(false);

        // Setting the player position and rotation

        GameObject player = GameManager.Instance.velo.gameObject;
        player.GetComponent<Rigidbody>().isKinematic = false;
        GameManager.Instance.SpawnPlayer();

        // Removing the black foreground
        oldUI.SetActive(true);
        StartCoroutine(FadeScreen(0f, fadeDuration, false));
        GameManager.Instance.SwitchState(GameState.QuestState);
    }

    void ShowScoreParam()
    {
        Vector3 startPos;
        switch (index)
        {
            case 0:
                startPos = tempsText.txt.rectTransform.position;
                LeanTween.moveX(tempsText.txt.gameObject, startPos.x + 20f, delays.scoreDisplay)
                    .setOnComplete(ShowScoreParam);
                tempsText.txt.LeanAlphaText(1, delays.scoreDisplay / 2f).setEaseInSine();
                break;
            case 1:
                startPos = echecText.txt.rectTransform.position;
                LeanTween.moveX(echecText.txt.gameObject, startPos.x + 20f, delays.scoreDisplay)
                    .setOnComplete(ShowScoreParam);
                echecText.txt.LeanAlphaText(1, delays.scoreDisplay / 2f).setEaseInSine();
                break;
            case 2:
                startPos = duraText.txt.rectTransform.position;
                LeanTween.moveX(duraText.txt.gameObject, startPos.x + 20f, delays.scoreDisplay)
                    .setOnComplete(ShowScoreParam);
                duraText.txt.LeanAlphaText(1, delays.scoreDisplay / 2f).setEaseInSine();
                break;
            case 3:
                index = -1;
                ShowTotal();
                break;
            default:
                break;
        }
        index++;
    }

    void ShowTotal()
    {
        totalText.Show();
        Invoke("CountScore", delays.scoreDisplay);
    }

    void CountScore()
    {
        pointNumberText.Show();
        pointNumberText.txt.text = "0";
        LeanTween.value(pointNumberText.txt.gameObject, 0f, score, delays.scoreNumberDuration)
            .setOnUpdate((float val) =>
            {
                pointNumberText.txt.text = "" + (int)val;
            });
        ptsText.Show();
        Invoke("HideScore", delays.scoreDuration);
    }

    void HideScore()
    {
        tempsText.txt.LeanAlphaText(0, delays.scoreDisplay).setOnComplete(ShowMedicalRecap);
        echecText.txt.LeanAlphaText(0, delays.scoreDisplay);
        duraText.txt.LeanAlphaText(0, delays.scoreDisplay);

        totalText.txt.LeanAlphaText(0, delays.scoreDisplay);
        pointNumberText.txt.LeanAlphaText(0, delays.scoreDisplay);
        ptsText.txt.LeanAlphaText(0, delays.scoreDisplay);
        StartCoroutine(FadeScreen(0f, fadeDuration, true));
    }

    void ShowMedicalRecap()
    {
        schemaHumain.img.gameObject.SetActive(true);
        Vector3 startPos;
        startPos = schemaHumain.img.rectTransform.position;
        LeanTween.moveX(schemaHumain.img.gameObject, startPos.x + 20, delays.medicalRecapSpeed)
            .setOnComplete(() =>
            {
                if (RecapManager.instance.medicalRecap.Injured())
                {
                    RecapManager.instance.medicalRecap.ShowMedicalRecap(delays.medicalRecapDuraction / 1.5f);
                }
                else
                {
                    LeanTween.value(schemaHumain.img.gameObject, schemaHumain.color, Color.green, 3f)
                    .setOnUpdate((Color c) =>
                    {
                        schemaHumain.img.color = c;
                        var tempColor = schemaHumain.img.color;
                        tempColor.a = 1f;
                        schemaHumain.img.color = tempColor;
                    });

                }

                Invoke("ShowRecap", delays.medicalRecapDuraction);
            });
        LeanTween.value(schemaHumain.img.gameObject, 0f, 1f, delays.medicalRecapSpeed / 1.5f)
            .setOnUpdate((float val) =>
            {
                Color c = schemaHumain.img.color;
                c.a = val;
                schemaHumain.img.color = c;
            });

    }

    void ShowRecap()
    {
        Debug.Log("We are showing the recap");
        Vector3 startPos = schemaHumain.img.rectTransform.position;
        LeanTween.moveX(schemaHumain.img.gameObject, startPos.x - 20, delays.medicalRecapSpeed)
            .setOnComplete(() =>
            {
                Invoke("ResetScene", delays.medicalRecapDuraction/2);
            });
        LeanTween.value(schemaHumain.img.gameObject, 1f, 0f, delays.medicalRecapSpeed)
            .setOnUpdate((float val) =>
            {
                Color c = schemaHumain.img.color;
                c.a = val;
                schemaHumain.img.color = c;
            });
    }

    void ResetScene()
    {
        StartCoroutine(FadeScreen(1f, fadeDuration, false));
        ResetUIData();
        Invoke("RenderWhite", fadeDuration + 2f);
    }

    void InitUIData()
    {
        tempsText.Init();
        echecText.Init();
        duraText.Init();

        totalText.Init();
        pointNumberText.Init();
        ptsText.Init();

        schemaHumain.Init();
    }

    void ResetUIData()
    {
        tempsText.Reset();
        echecText.Reset();
        duraText.Reset();

        totalText.Reset();
        pointNumberText.Reset();
        ptsText.Reset();

        schemaHumain.Reset();
    }

    public void FadeTransition(float endAlpha, float speed, float duration)
    {
        StartCoroutine(FadeScreenBounce(endAlpha, speed, duration, false));
    }

    IEnumerator FadeScreenBounce(float endAlpha, float speed, float duration, bool hideGameobject)
    {
        float a = fadeScreenImage.color.a;
        Color c = fadeScreenImage.color;

        if (!hideGameobject)
        {
            fadeScreenImage.gameObject.SetActive(true);
        }
        else
        {
            fadeScreenImage.gameObject.SetActive(false);
        }

        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / speed)
        {
            c.a = Mathf.Lerp(a, endAlpha, i);
            fadeScreenImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadeScreenImage.color = c;
        yield return new WaitForSeconds(duration);

        a = fadeScreenImage.color.a;
        c = fadeScreenImage.color;
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / speed)
        {
            c.a = Mathf.Lerp(a, 1-endAlpha, i);
            fadeScreenImage.color = c;
            yield return null;
        }
        c.a = 1-endAlpha;
        fadeScreenImage.color = c;
    }

    public void FadeDamage(float intensity)
    {
        fadeDamageImage.gameObject.SetActive(true);
        LeanTween.value(fadeDamageImage.gameObject, 0f, intensity, 0.2f).
            setOnUpdate((float v) =>
            {
                Color c = fadeDamageImage.color;
                c.a = v;
                fadeDamageImage.color = c;
            }).setOnComplete(() =>
            {
                LeanTween.value(fadeDamageImage.gameObject, intensity, 0f, 1f).
                    setOnUpdate((float t) =>
                    {
                        Color c = fadeDamageImage.color;
                        c.a = t;
                        fadeDamageImage.color = c;
                    }).
                        setEaseInBack().
                            setOnComplete(() =>
                            {
                                fadeDamageImage.gameObject.SetActive(false);
                            });
            });
    }
}
