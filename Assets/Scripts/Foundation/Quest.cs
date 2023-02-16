using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : Interactable
{
    public List<Task> requiredTasks;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.remainingQuests.Add(this);
        GetComponent<Renderer>().material.SetColor("_Color", GameManager.Instance.interactionProperties.taskColor);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        HideInteractable();
        NextTask();
    }

    public void CompleteTask(Task task)
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
        }
    }

    void NextTask()
    {
        requiredTasks[0].ShowInteractable();
        requiredTasks[0].SetQuest(this);
        foreach (var item in requiredTasks[0].requiredObjects)
        {
            item.ShowInteractable();
        }
    }
}
