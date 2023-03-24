using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semer : Quest
{
    [SerializeField] Remorque remorque;

    public override void CompleteTask(Task task)
    {
        base.CompleteTask(task);
        if (requiredTasks.Count <= 0)
        {
            remorque.DetachRemorque();
        }
    }
}

