using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

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
    public Item[] necessaryItem; // Le meilleur objet
    protected Quest quest;
    public bool succeeded { get; private set; }

    [Header("Switching")]
    [SerializeField] protected bool switchPlayer;
    [SerializeField] protected string targetChange;
    

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
            HandleSucceededTask();
        }
        else
        {
            if (CheckNecessaryItem()) // Check if all the necessary items are collected
            {
                HandleSucceededTask();
                Debug.Log("All necessary are aquired");
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
                    HandleSucceededTask();
                }
                
            }
        }
    }

    public virtual void SetQuest(Quest q)
    {
        quest = q;
    }

    protected virtual void HandleFailedTask()
    {
        succeeded = false;
        quest.CompleteTask(this);
        GameManager.Instance.GetComponent<TransitionManager>().FadeDamage(0.12f);
        Debug.Log("Task failed");
        GameManager.Instance.FailTask();
        //GameManager.Instance.velo.GetComponent<DamageController>().DamageTractor(5f);
    }

    protected virtual void HandleSucceededTask()
    {
        succeeded = true;
        quest.CompleteTask(this);
        GameManager.Instance.SucceedTask(this);
    }

    protected bool CheckNecessaryItem()
    {
        return necessaryItem.All(element => GameManager.Instance.collectedItems.Contains(element));
    }

    protected void HideAllNecessaryItems()
    {
        for (int i = 0; i<necessaryItem.Length; i++)
        {
            necessaryItem[i].HideInteractable();
        }
    }
}
