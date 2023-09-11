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
            counting = false;
            GameManager.Instance.CompleteQuest(this,elapsedTime);
            GameManager.Instance.currentQuest = null;

            DataManager.instance.UpdateVisiteurData(client);
        }
    }
}
