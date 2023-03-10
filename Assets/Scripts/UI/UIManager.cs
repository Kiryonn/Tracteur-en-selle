using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] TextMeshProUGUI textPrefab;
    [SerializeField] Canvas canva;
    [SerializeField] Transform questRoot;
    Dictionary<Quest, TextMeshProUGUI> questDico;
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
}
