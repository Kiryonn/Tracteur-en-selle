using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTask : Task
{
    bool check;
    public override void ShowInteractable()
    {
        if (check) base.ShowInteractable();
    }

    protected override void OnStart()
    {
        base.OnStart();
        GameManager.Instance.onCollectedItem.AddListener(CheckItem);
    }

    void CheckItem(Item i)
    {
        if (requiredObjects.Contains(i))
        {
            check = true;
            ShowInteractable();
        }
    }
}
