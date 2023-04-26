using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraiteQuete : Quest
{
    public TraiteInteractible levier;
    [SerializeField] GameObject tractor;
    protected override void StartQuest()
    {
        base.StartQuest();
        levier.ShowInteractable();
        StartCoroutine(GameManager.Instance.player.SwitchControls("Character",true));
        Invoke("ShowTractor", 3f);
    }

    protected override void OnStart()
    {
        base.OnStart();
        tractor.SetActive(false);
    }

    void ShowTractor()
    {
        tractor.SetActive(true);
    }

    void HideTractor()
    {
        tractor.SetActive(false);
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
            StartCoroutine(GameManager.Instance.player.SwitchControls("Tractor",true));
            Invoke("HideTractor", 3f);
            GameManager.Instance.currentQuest = null;
        }
    }
}
