using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : Interactable
{
    [ColorUsage(true,true)]
    [SerializeField] Color customColor;
    [SerializeField] bool customizeColor;
    public bool requireItem;
    [HideInInspector]
    [SerializeField]
    public List<Item> requiredObjects; // Tous les objets qui pourrait être utilisé
    public float sucessChance;
    public Item necessaryItem; // Le meilleur objet
    protected Quest quest;
    protected override void OnStart()
    {
        base.OnStart();
        GameManager.Instance.remainingTasks.Add(this);
        if (customizeColor)
        {
            render.material.SetColor("_Color", customColor);
        }
        else
        {
            render.material.SetColor("_Color", GameManager.Instance.interactionProperties.taskColor);
        }
        HideInteractable();
    }

    public override void Interact()
    {
        base.Interact();
        foreach (var item in requiredObjects)
        {
            item.HideInteractable();
        }
        if (!requireItem)
        {
            quest.CompleteTask(this);
        }
        else
        {
            if (GameManager.Instance.collectedItems.Contains(necessaryItem))
            {
                quest.CompleteTask(this);
            }
            else
            {
                int r = Random.Range(0, 100);
                if (r > sucessChance)
                {
                    HandleFailedTask();
                }
                else
                {
                    Debug.Log("Task sucessfully not failed");
                }
                quest.CompleteTask(this);
            }
        }

    }

    public virtual void SetQuest(Quest q)
    {
        quest = q;
    }

    protected virtual void HandleFailedTask()
    {
        Debug.Log("Task failed sucessfully");
        GameManager.Instance.FailTask();
        GameManager.Instance.velo.GetComponent<DamageController>().DamageTractor(5f);
    }
}
