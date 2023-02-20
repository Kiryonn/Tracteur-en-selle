using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : Interactable
{
    [SerializeField] protected List<Task> requiredTasks;
    public MSAVision vision;
    protected override void OnStart()
    {
        GameManager.Instance.remainingQuests.Add(this);
        if (vision) { vision.HideInteractable(); }
        GetComponent<Renderer>().material.SetColor("_Color", GameManager.Instance.interactionProperties.taskColor);
    }

    public override void Interact()
    {
        base.Interact();
        StartQuest();
    }

    public void StartQuest()
    {
        if (requiredTasks.Count <= 0)
        {
            GameManager.Instance.CompleteQuest(this);
            return;
        }
        GameManager.Instance.HideAllObjectsOfType(typeof(Quest));
        GameManager.Instance.remainingTasks = requiredTasks;
        GameManager.Instance.currentQuest = this;

        HideInteractable();
        NextTask();
    }

    public virtual void CompleteTask(Task task)
    {
        task.HideInteractable();
        requiredTasks.Remove(task);
        if (requiredTasks.Count > 0)
        {
            NextTask();
        }
        else
        {
            GameManager.Instance.CompleteQuest(this);
            GameManager.Instance.currentQuest = null;
        }
    }

    void NextTask()
    {
        requiredTasks[0].ShowInteractable();
        requiredTasks[0].SetQuest(this);
        if (requiredTasks[0].requireItem)
        {
            foreach (var item in requiredTasks[0].requiredObjects)
            {
                item.ShowInteractable();
            }
            if (vision) { vision.ShowInteractable(); }
        }
        else
        {
            if (vision) { vision.HideInteractable(); }
        }
        
    }

    public Task GetCurrentTask()
    {
        return requiredTasks[0];
    }
}
