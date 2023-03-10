using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraiteQuete : Quest
{
    public TraiteInteractible levier;

    protected override void StartQuest()
    {
        base.StartQuest();
        levier.ShowInteractable();
    }

    public override void CompleteTask(Task task)
    {
        task.HideInteractable();
        requiredTasks.Remove(task);
        if (requiredTasks.Count > 0)
        {
            NextTask();
        }
        else
        {
            levier.HideInteractable();
            GameManager.Instance.CompleteQuest(this);
            GameManager.Instance.currentQuest = null;
        }
    }
}
