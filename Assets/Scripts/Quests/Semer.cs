using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semer : Quest
{
    [SerializeField] Remorque remorque;
    NightTime nTime;
    protected override void OnStart()
    {
        base.OnStart();
        if (GameManager.Instance != null)
        {
            nTime = GameManager.Instance.gameObject.GetComponent<NightTime>();
            nTime.dayNightCycle = true;
        }
        
    }

    public override void Interact()
    {
        base.Interact();
        nTime.FadeDayNight(50f, 0f);
    }

    public override void CompleteTask(Task task)
    {
        base.CompleteTask(task);
        if (requiredTasks.Count <= 0)
        {
            remorque.DetachRemorque();
            nTime.FadeDayNight(20f, 1f);
            //nTime.SetDayTime(true);
        }
    }
}

