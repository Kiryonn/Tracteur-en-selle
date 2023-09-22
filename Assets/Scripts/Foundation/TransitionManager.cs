using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

// Gère les transitions entre des éléments de l'ui comme un flash de dégat, un fondu au noir
// Gère aussi la phase où le score s'affiche en baissant la luminosité au max

public class TransitionManager : MonoBehaviour
{
    public Light globalLight;
    public Light spot;
    public Transform tractorPosition;
    public GameObject oldUI;

    [Header("important")]
    [SerializeField] int transitionIndex;

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

    [Header("UI Elements Stats")]

    [SerializeField] CanvasGroup globalCanva; // Contains all the stats ui elements

    public UIDataText filliereName;

    public GameObject circonstanceLayout; // Vertical Layout where you piss text inside
    //public UIDataText circonsMockupText;
    public TextMeshProUGUI circonsText;

    public TextMeshProUGUI coutATF; // Contains the image with color of the pie (you need to change it)
    public TextMeshProUGUI coutATBT; // same as above

    public TextMeshProUGUI coutAn;
    public TextMeshProUGUI coutJ;

    public Sprite circleSprite; // Pie form
    [SerializeField] CanvasGroup pieExplain; //
    [SerializeField] PieCreator pieCreator; // Contains the alpha group

    float score;
    int index = 0;
    int indexStat = 0;

    [Header("Musics")]
    [SerializeField] AudioClip normalMusic;
    [SerializeField] AudioClip victoryMusic;

    [Header("Data")]
    [SerializeField] StatistiqueManager statistiqueManager;

    [Header("Leaderboard")]
    [SerializeField] CanvasGroup scoreRoot;
    [SerializeField] RectTransform scoreContainer;
    [SerializeField] GameObject scorePrefab;

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
        InitUIData();

        // Fade screen from black to white
        StartCoroutine(FadeScreen(0f, fadeDuration, false));
    }

    public void SetValues(float t, float e, float s, float scr)
    {
        score = scr;
        tempsText.txt.text = "Temps : " + (int)t + " secondes";
        echecText.txt.text = "Echecs : " + e;
        duraText.txt.text = "Sante du Tracteur : " + s + "%";
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
        AudioManager.instance.ChangeBackgroundMusic(victoryMusic);
        Invoke("RenderBlack", fadeDuration + 2f);
    }

    public void RenderBlack()
    {
        MyDebug.Log("We are rendering black");
        bool check = !SettingsManager.instance.isGarageOrTutorial();

        // Making the scene black

        Camera.main.enabled = false;
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

        if (check) { StartCoroutine(MakeNextTransition()); } else { RenderWhite(); }
        //AudioManager.instance.ChangeBackgroundMusic(victoryMusic);

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
        GameManager.Instance.SwitchState(GameState.QuestState);
        AudioManager.instance.ChangeBackgroundMusic(normalMusic);

        SettingsManager.instance.LoadNextLevel();
        SettingsManager.instance.LoadMainMenu();
        //Camera.main.enabled = true;
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
                transitionIndex++;
                StartCoroutine(MakeNextTransition());
                break;
            default:
                break;
        }
        index++;
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
    }

    void HideScore()
    {
        tempsText.txt.LeanAlphaText(0, delays.scoreDisplay);
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
        LeanTween.moveX(schemaHumain.img.gameObject, startPos.x + 18, delays.medicalRecapSpeed)
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
                //StartCoroutine(WaitForInputs());
                //Invoke("ShowRecap", delays.medicalRecapDuraction);
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
        MyDebug.Log("We are showing the recap");
        Vector3 startPos = schemaHumain.img.rectTransform.position;
        LeanTween.moveX(schemaHumain.img.gameObject, startPos.x - 20, delays.medicalRecapSpeed)
            .setOnComplete(() =>
            {
                Invoke("ResetScene", delays.medicalRecapDuraction / 2);
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
        // score
        tempsText.Init();
        echecText.Init();
        duraText.Init();

        totalText.Init();
        pointNumberText.Init();
        ptsText.Init();
        // medical
        schemaHumain.Init();

        // stats
        filliereName.Init();
        globalCanva.alpha = 0f;
        //circonsMockupText.Init();

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

        // stats
        filliereName.Reset();
        globalCanva.alpha = 0f;
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
            c.a = Mathf.Lerp(a, 1 - endAlpha, i);
            fadeScreenImage.color = c;
            yield return null;
        }
        c.a = 1 - endAlpha;
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

    void HideMedicalRecap()
    {
        Vector3 startPos = schemaHumain.img.rectTransform.position;
        LeanTween.moveX(schemaHumain.img.gameObject, startPos.x - 20, delays.medicalRecapSpeed)
            .setOnComplete(() =>
            {
                RecapManager.instance.medicalRecap.ClearInjuries();
                //transitionIndex++;
                //StartCoroutine(MakeNextTransition());
            });
        LeanTween.value(schemaHumain.img.gameObject, 1f, 0f, delays.medicalRecapSpeed)
            .setOnUpdate((float val) =>
            {
                Color c = schemaHumain.img.color;
                c.a = val;
                schemaHumain.img.color = c;
            });
    }

    IEnumerator WaitForInputs()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        StartCoroutine(MakeNextTransition());
    }

    IEnumerator MakeNextTransition()
    {
        switch (transitionIndex)
        {
            case 0:
                ShowScoreParam();
                break;
            case 1:
                totalText.Show();
                yield return new WaitForSeconds(delays.scoreDisplay);
                transitionIndex++;
                StartCoroutine(MakeNextTransition());
                break;
            case 2:
                CountScore();
                yield return new WaitForSeconds(delays.scoreDuration);
                transitionIndex++;
                StartCoroutine(MakeNextTransition());
                break;
            case 3:
                HideScore();
                yield return new WaitForSeconds(delays.scoreDuration / 2);
                transitionIndex++;
                StartCoroutine(MakeNextTransition());
                break;
            case 4:
                ShowMedicalRecap();
                transitionIndex++;
                StartCoroutine(WaitForInputs());
                break;
            case 5:
                //HideMedicalRecap();
                transitionIndex++;
                StartCoroutine(MakeNextTransition());
                break;
            case 6:
                if (SettingsManager.instance.settings.allowStats)
                {
                    transitionIndex = 8;
                    StartCoroutine(MakeNextTransition());
                }
                else
                {
                    transitionIndex++;
                    StartCoroutine(ShowStats());
                }
                
                break;
            case 7:
                HideStats();
                yield return new WaitForSeconds(3f);
                StartCoroutine(MakeNextTransition());
                break;
            case 8:
                ShowLeaderboard();
                transitionIndex++;
                StartCoroutine(WaitForInputs());
                break;
            default:
                ResetScene();
                break;
        }
        //if (_continueT) { StartCoroutine(MakeNextTransition()); }

        yield return null;
    }

    void ShowLeaderboard()
    {
        LeanTween.value(0f, 1f, 1f).setOnUpdate((float val) =>
        {
            scoreRoot.alpha = val;
        });
        int i = 1;
        Dictionary<string, (float,float)> myDic = SettingsManager.instance.scoreDataManager.scores.OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);

        RectTransform temp;

        
        foreach (KeyValuePair<string, (float,float)> entry in myDic)
        {
            if (i > 10) return;
           
            temp = Instantiate(scorePrefab).GetComponent<RectTransform>();
            temp.SetParent(scoreContainer);
            temp.localScale = Vector3.one;
            temp.anchoredPosition3D = new Vector3(temp.anchoredPosition3D.x, temp.anchoredPosition3D.y, 0f);

            TextMeshProUGUI[] values = temp.GetComponentsInChildren<TextMeshProUGUI>();

            values[0].text = "-" + i + "-";
            values[1].text = entry.Key;
            values[2].text = Mathf.RoundToInt(entry.Value.Item1) + "";

            i++;
        }
    }

    IEnumerator ShowStats()
    {
        if (indexStat >= SettingsManager.instance.settings.currentTheme.Count)
        {
            StartCoroutine(MakeNextTransition());
            yield break;
        }

        LeanTween.value(schemaHumain.img.gameObject, schemaHumain.color, Color.white, 3f)
                    .setOnUpdate((Color c) =>
                    {
                        schemaHumain.img.color = c;
                        var tempColor = schemaHumain.img.color;
                        tempColor.a = 1f;
                        schemaHumain.img.color = tempColor;
                    });

        DataManager.instance
            .GetComponent<StatistiqueManager>()
                .filliereToStatsDictionary
                    .TryGetValue(SettingsManager.instance.settings.currentTheme[indexStat], out Statistique st);

        filliereName.txt.text = SettingsManager.instance.settings.currentTheme[indexStat].ToString();
        filliereName.Show();

        coutAn.text = "Coût moyen d'un AT par AN en euros : " + st.CoutAn + " €";
        coutJ.text = "Coût moyen d'un AT par JOUR en euros : " + st.CoutJour + " €";

        Color[] colors = PieCreator.CreatePie(pieCreator.gameObject, circleSprite, new int[] { st.ATBT, st.ATFrance });

        coutATBT.text = "Nombre d'AT en région berry-tourainne : " + st.ATBT;
        coutATF.text = "Nombre d'AT en France : " + st.ATFrance;

        coutATBT.GetComponentInChildren<Image>().color = colors[0];
        coutATF.GetComponentInChildren<Image>().color = colors[1];

        RecapManager.instance.medicalRecap.ClearInjuries();

        for (int i = 0; i < st.Zones.Length; i++)
        {
            Parts parsedPart = (Parts)System.Enum.Parse(typeof(Parts), st.Zones[i].ID);
            RecapManager.instance.medicalRecap.AddInjurie(parsedPart, st.Zones[i].Valeur);
        }

        RecapManager.instance.medicalRecap.ShowMedicalRecap(10f);

        circonsText.gameObject.SetActive(true);

        for (int i = circonstanceLayout.transform.childCount - 1; i > 0; i--)
        {
            Destroy(circonstanceLayout.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < st.Circonstance.Length; i++)
        {
            TextMeshProUGUI tempText = Instantiate(circonsText, circonsText.rectTransform.parent);
            tempText.text = (st.Circonstance[i].Motif + " : " + st.Circonstance[i].Pourcentage + "%");

            //st.Circonstance[i].
        }

        circonsText.gameObject.SetActive(false);

        LeanTween.value(0, 1, 3f).setOnUpdate((float value) =>
        {
            globalCanva.alpha = value;
        });
        //LeanTween.moveX(globalCanva.gameObject,)

        indexStat++;
        yield return new WaitForSeconds(3f);
        StartCoroutine(WaitForInputs());
    }

    void HideStats()
    {
        if (globalCanva.alpha == 0)
        {
            transitionIndex++;
            HideMedicalRecap();
        }
        else
        {
            transitionIndex--;
            LeanTween.value(1, 0, 3f).setOnUpdate((float value) =>
            {
                globalCanva.alpha = value;
            });
        }

    }
}
