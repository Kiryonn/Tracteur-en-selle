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
