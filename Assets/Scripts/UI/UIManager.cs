using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpeedSystem
{
    public ParticleSystem particleSystem;
    public float maxParticle;
    
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] TextMeshProUGUI textPrefab;
    [SerializeField] Canvas canva;
    [SerializeField] Transform questRoot;
    Dictionary<Quest, TextMeshProUGUI> questDico;
    [SerializeField] Image progressBar;
    [SerializeField] CanvasGroup progressRoot;
    [SerializeField] GameObject energyRoot;
    public GameObject pauseMenu;
    public TextMeshProUGUI timerText;
    public SpeedSystem speedSystem;
    bool fading;
    [Header("Quete en cour")]
    [SerializeField] GameObject tacheRoot;
    [SerializeField] TextMeshProUGUI queteName;
    [SerializeField] TextMeshProUGUI tacheName;

    [Header("Task bar stylish")]
    [SerializeField] RectTransform bar;
    [SerializeField] TextMeshProUGUI barText;
    [SerializeField] Image leftIcon;
    [SerializeField] Image rightIcon;
    [SerializeField] float maxWidth;
    [SerializeField] float minWidth;
    [SerializeField] Image obscurImage;
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
        questDico = new Dictionary<Quest, TextMeshProUGUI>();
        questRoot.gameObject.SetActive(true);
        tacheRoot.SetActive(false);
        HideStylishBar(true);
    }

    public void SetListener(ResourceController res)
    {
        res.energyChangeEvent.AddListener(UpdateEnergy);
        res.speedChangeEvent.AddListener(UpdateSpeed);
    }

    public void SetProgressListener(AreaOfUse area)
    {
        area.onProgressChanged.AddListener(UpdateProgress);
        //Debug.Log("Progress is set");
        StopCoroutine("FadeProgress");
        StartCoroutine(FadeProgress(1f,1f));
    }

    public void RemoveProgressListener(AreaOfUse area)
    {
        area.onProgressChanged.RemoveListener(UpdateProgress);
        StopCoroutine("FadeProgress");
        StartCoroutine(FadeProgress(0f, 1f));
    }

    void UpdateProgress(float amount)
    {
        progressBar.fillAmount = amount;
        if (amount == 1f) StartCoroutine(FadeProgress(0f, 2f));
    }

    public void SetQuestListener()
    {
        GameManager.Instance.onCreatedQuest.AddListener(UpdateQuest);
        GameManager.Instance.onStartQuest.AddListener(UpdateTabletQuest);
        GameManager.Instance.onCompleteTask.AddListener(UpdateQueteEnCour);
        GameManager.Instance.onCompleteQuest.AddListener(UpdateFinishQuest);
    }

    void UpdateEnergy(PlayerUI ui, float amount)
    {
        ui.energy.fillAmount = amount;
    }

    void UpdateSpeed(PlayerUI ui, float amount)
    {
        ui.speed.fillAmount = amount;
    }

    void UpdateQuest(Quest q,string txt)
    {
        //Debug.Log("Trying to bind " + q._name);
        if (questDico.ContainsKey(q))
        {
            questDico[q].gameObject.SetActive(false);
        }
        else
        {
            TextMeshProUGUI text = Instantiate(textPrefab);
            text.transform.SetParent(questRoot, false);
            text.text = "- " + txt;
            questDico.TryAdd(q, text);
        }
    }

    void UpdateTabletQuest(Quest q)
    {
        questRoot.gameObject.SetActive(false);
        queteName.text = q._name;
        tacheName.text = q.GetCurrentTask()._name;
        tacheRoot.gameObject.SetActive(true);
        Invoke("ShowStylishBar", 1f);
    }

    void UpdateQueteEnCour(Quest q)
    {
        try
        {
            tacheName.text = q.GetCurrentTask()._name;
        }
        catch
        {
            Debug.Log("Np");
        }
    }

    public void ShowStylishBar()
    {
        barText.text = queteName.text;
        LeanTween.value(rightIcon.gameObject, rightIcon.color.a, 1f, 0.5f)
            .setOnUpdate((float val) =>
            {
                rightIcon.SetAlpha(val);
                leftIcon.SetAlpha(val);
                
            });
        LeanTween.value(barText.gameObject, 0f, 1f, 1.5f)
            .setDelay(2f)
            .setOnUpdate((float val) =>
            {
                barText.alpha = val;
            });
        LeanTween.size(bar, new Vector2(maxWidth, bar.sizeDelta.y), 2f).setEaseInBack();
        LeanTween.value(obscurImage.gameObject, 0f, 1f, 2f).setDelay(0.2f)
            .setOnUpdate((float val) =>
            {
                obscurImage.SetAlpha(val);
            });
        // Just to do a delay
        LeanTween.value(gameObject, 0f, 1f, 1f).setDelay(4f).setOnComplete(() => { HideStylishBar(); });
    }

    void HideStylishBar(bool fastHide = false)
    {
        if (fastHide)
        {
            bar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
            leftIcon.SetAlpha(0f);
            rightIcon.SetAlpha(0f);
            barText.alpha = 0f;
            Color c = obscurImage.color;
            c.a = 0f;
            obscurImage.color = c;
        }
        else
        {
            LeanTween.value(rightIcon.gameObject, rightIcon.color.a, 0f, 0.5f).setDelay(1.5f)
            .setOnUpdate((float val) =>
            {
                rightIcon.SetAlpha(val);
                leftIcon.SetAlpha(val);
                obscurImage.SetAlpha(val);
            });
            LeanTween.value(barText.gameObject, 1f, 0f, 1.5f).setDelay(0.3f)
                .setOnUpdate((float val) =>
                {
                    barText.alpha = val;
                });
            LeanTween.size(bar, new Vector2(minWidth, bar.sizeDelta.y), 2f).setEaseInBack();
        }
    }

    void UpdateFinishQuest(Quest q)
    {
        questRoot.gameObject.SetActive(true);
        tacheRoot.SetActive(false);
    }

    IEnumerator FadeProgress(float endAlpha, float duration)
    {
        fading = true;
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / duration)
        {
            progressRoot.alpha = Mathf.Lerp(1-endAlpha, endAlpha, i);
            yield return null;
        }
        progressRoot.alpha = endAlpha;
        fading = false;
    }

    public void HideEnergy(bool yn)
    {
        energyRoot.SetActive(!yn);
    }


}

public static class ImageEx
{
    public static Image SetAlpha(this Image image,float to)
    {
        Color c =  image.color;
        c.a = to;
        image.color = c;
        return image;
    }
}
