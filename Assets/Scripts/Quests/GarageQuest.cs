using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageQuest : Quest
{
    public float tractorDamage;

    protected override void OnStart()
    {
        base.OnStart();
        GameManager.Instance.player.isCharacterControlled = true;
        //StartCoroutine(GameManager.Instance.player.SwitchControls("Character", false));
    }

    protected override void NextTask()
    {
        foreach (var item in requiredTasks)
        {
            item.ShowInteractable();
            for(int i=0; i< item.necessaryItem.Length; i++)
            {
                item.necessaryItem[i].ShowInteractable();
                item.SetQuest(this);
            }
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
            StartCoroutine(GameManager.Instance.player.SwitchControls("Character", true));
            
        }
    }
}
