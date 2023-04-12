using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorielQuest : Quest
{
    public ClientData client;
    protected override void OnStart()
    {
        base.OnStart();
        GameManager.Instance.player.SwitchControls("Character");
        client = new ClientData();
        client.ID = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
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
            if (vision)
            {
                vision.HideInteractable();
            }
            GameManager.Instance.CompleteQuest(this);
            GameManager.Instance.currentQuest = null;

            DataManager.instance.UpdateVisiteurData(client);
            StartCoroutine(GameManager.Instance.player.SwitchControls("Tractor"));
        }
    }
}
