using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageTask : Task
{
    bool check;
    protected override void OnStart()
    {
        base.OnStart();
        GameManager.Instance.onCollectedItem.AddListener(CheckItem);
    }
    public override void ShowInteractable()
    {
        if (check)
        {
            base.ShowInteractable();
        }
    }

    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.velo.GetComponent<DamageController>().HealTractor(20f);
    }

    void CheckItem(Item i)
    {
        if (CheckNecessaryItem())
        {
            check = true;
            ShowInteractable();
        }
    }
}
