using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuveQuest : Quest
{
    [HideInInspector]
    public CleaningMaterial cleaningMaterial;
    [SerializeField] GameObject tractorObject;
    bool check;
    [SerializeField] GazZone gazZone;

    
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
            GameManager.Instance.player.GetComponent<KarcherController>().RemoveKarcher();
            gazZone.co2UI.activated = false;
            GameManager.Instance.HideUIObject(gazZone.co2UI.gameObject);
        }
        else
        {
            check = true;
        }
    }
}
