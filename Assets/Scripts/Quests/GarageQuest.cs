using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageQuest : Quest
{
    public float tractorDamage;
    [SerializeField] GameObject damagedTractor;
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
            }
            item.SetQuest(this);
        }
    }

    public override void CompleteTask(Task task)
    {
        task.HideInteractable();
        requiredTasks.Remove(task);

        if (requiredTasks.Count == 1)
        {
            StartCoroutine(GameManager.Instance.player.SwitchControls("Tractor", true));
            requiredTasks[requiredTasks.Count-1].ShowInteractable();
            Invoke("HideTractor", 3f);
        }

        if (requiredTasks.Count <= 0)
        {
            GameManager.Instance.CompleteQuest(this);
            GameManager.Instance.currentQuest = null;
            GameManager.Instance.player.isCharacterControlled = false;

        }
    }

    void HideTractor()
    {
        damagedTractor.SetActive(false);
    }
}
