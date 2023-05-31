using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptPosTask : Task
{
    public SelectBetailOption currentOption { get; private set; }

    public override void ShowInteractable()
    {
        if (currentOption)
        {
            base.ShowInteractable();
        }
    }

    public override void Interact()
    {
        base.Interact();
        currentOption.FadeHologram();
    }

    protected override void ItemCollectedTrigger(Item item)
    {
        if (GameManager.Instance.currentQuest.GetCurrentTask() != this) { return; }

        base.ItemCollectedTrigger(item);

        if (!currentOption)
        {
            currentOption = (SelectBetailOption)item;
            ShowInteractable();
        }
        else
        {
            currentOption = (SelectBetailOption)item;
        }
        
        Debug.Log("Betail collected");
        foreach (var require in requiredObjects)
        {
            if (require != item)
            {
                require.ShowInteractable();
                SelectBetailOption selectBetailOption = (SelectBetailOption)require;
                selectBetailOption.RemoveHologram();
                GameManager.Instance.collectedItems.Remove(require);
            }
        }
    }
}
