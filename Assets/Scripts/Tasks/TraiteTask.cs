using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraiteTask : Task
{
    public override void Interact()
    {
        TraiteQuete tq = (TraiteQuete)quest;
        if (tq.levier.activated)
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
