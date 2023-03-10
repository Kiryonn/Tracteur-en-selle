using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageQuest : Quest
{
    public float tractorDamage;
    protected override void NextTask()
    {
        foreach (var item in requiredTasks)
        {
            item.ShowInteractable();
            item.necessaryItem.ShowInteractable();
            item.SetQuest(this);
        }
    }

    public override void CompleteTask(Task task)
    {
        task.HideInteractable();
        requiredTasks.Remove(task);
        if (requiredTasks.Count <= 0)
        {
            GameManager.Instance.CompleteQuest(this);
            GameManager.Instance.currentQuest = null;
        }
    }
}
