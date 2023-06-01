using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NettoyageCuve : Task
{
    bool gotCleaningMats;
    CuveQuest q;
    public override void ShowInteractable()
    {
        if (!gotCleaningMats) { return; }
        base.ShowInteractable();
    }

    public override void Interact()
    {
        if (q.cleaningMaterial.used)
        {
            base.Interact();
        }
        else
        {
            q.cleaningMaterial.Use(this);
        }
    }

    protected override void ItemCollectedTrigger(Item item)
    {
        if (GameManager.Instance.currentQuest.GetCurrentTask() != this) { return; }
        q = (CuveQuest) GameManager.Instance.currentQuest;
        if (requiredObjects.Contains(item))
        {
            gotCleaningMats = true;
            q.cleaningMaterial = (CleaningMaterial)item;
            ShowInteractable();
        }
    }
}
