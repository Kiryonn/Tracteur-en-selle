using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailleHaieQuest : Quest
{
    [SerializeField] RecupEpareuse epareuse;
    [SerializeField] Plots pl;

    public override void CompleteTask(Task task)
    {
        base.CompleteTask(task);
        if (requiredTasks.Count <= 0)
        {
            epareuse.DetachEpareuse();
            if (pl)
            {
                pl.RemovePlots();
            }
        }
    }
}
