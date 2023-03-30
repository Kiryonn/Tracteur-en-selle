using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] TextMeshProUGUI textPrefab;
    [SerializeField] Canvas canva;
    [SerializeField] Transform questRoot;
    Dictionary<Quest, TextMeshProUGUI> questDico;
    [SerializeField] Image progressBar;
    [SerializeField] CanvasGroup progressRoot;
    bool fading;
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
    }

    public void SetListener(ResourceController res)
    {
        res.energyChangeEvent.AddListener(UpdateEnergy);
        res.speedChangeEvent.AddListener(UpdateSpeed);
    }

    public void SetProgressListener(AreaOfUse area)
    {
        area.onProgressChanged.AddListener(UpdateProgress);
        
        StartCoroutine(FadeProgress(1f,1f));
    }

    public void RemoveProgressListener(AreaOfUse area)
    {
        area.onProgressChanged.RemoveListener(UpdateProgress);
        if (!fading) StartCoroutine(FadeProgress(0f, 1f));
    }

    void UpdateProgress(float amount)
    {
        progressBar.fillAmount = amount;
        if (amount == 1f) StartCoroutine(FadeProgress(0f, 2f));
    }

    public void SetQuestListener()
    {
        GameManager.Instance.onCreatedQuest.AddListener(UpdateQuest);
    }

    void UpdateEnergy(PlayerUI ui, float amount)
    {
        ui.energy.fillAmount = amount * 0.655f + 0.2f;
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
}
