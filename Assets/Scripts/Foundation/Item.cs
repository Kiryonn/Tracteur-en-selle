using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public List<Item> linkedItems;
    private void Start()
    {
        OnStart();
    }
    public override void Interact()
    {
        base.Interact();
        HideInteractable();
        GameManager.Instance.CollectItem(this);
        foreach (var item in linkedItems)
        {
            item.HideInteractable();
        }
    }

    public virtual void OnStart()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", GameManager.Instance.interactionProperties.itemColor);
        GameManager.Instance.allItems.Add(this);
        HideInteractable();
    }
}
