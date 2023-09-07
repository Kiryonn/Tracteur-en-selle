using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public List<Item> linkedItems;
    protected Quest quest;
    public bool noDroneRequest;
    public override void Interact()
    {
        base.Interact();
        HideInteractable();
        GameManager.Instance.CollectItem(this);
        AudioManager.instance.PlaySFX(AudioManager.instance.soundData.recupClip);
        foreach (var item in linkedItems)
        {
            item.HideInteractable();
        }
    }

    public override void HideInteractable()
    {
        base.HideInteractable();
    }

    protected override void OnStart()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", GameManager.Instance.interactionProperties.itemColor);
        GameManager.Instance.allItems.Add(this);
        HideInteractable();
    }

    public override void ShowInteractable()
    {
        base.ShowInteractable();
        
    }

    public void SetQuest(Quest q)
    {
        quest = q;
    }
}
