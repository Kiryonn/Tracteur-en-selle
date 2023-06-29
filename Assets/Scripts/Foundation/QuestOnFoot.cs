using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOnFoot : Quest
{
    [SerializeField] GameObject tractorObject;
    protected bool check;

    protected override void OnStart()
    {
        base.OnStart();
        HideTractor();
    }
    public override void Interact()
    {
        base.Interact();
        StartCoroutine(GameManager.Instance.player.SwitchControls("Character", true));
        Invoke("ShowTractor", 3f);
    }

    protected override void HandleCompletedQuest()
    {
        base.HandleCompletedQuest();
        StartCoroutine(GameManager.Instance.player.SwitchControls("Tractor", true));
        Invoke("HideTractor", 3f);
    }

    void ShowTractor()
    {
        tractorObject.SetActive(true);
    }

    void HideTractor()
    {
        tractorObject.SetActive(false);
        if (check)
        {
            HandleExitQuest();
        }
        else
        {
            check = true;
        }
    }

    protected virtual void HandleExitQuest()
    {

    }
}
