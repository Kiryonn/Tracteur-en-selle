using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : Interactable
{
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
        render.material.SetColor("_Color", GameManager.Instance.interactionProperties.taskColor);
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
                    Debug.Log("Task failed sucessfully");
                    quest.CompleteTask(this);
                }

            }
        }

    }

    public virtual void SetQuest(Quest q)
    {
        quest = q;
    }
}
