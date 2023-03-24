using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public List<Item> linkedItems;
    [SerializeField] Transform pin;
    public bool noDroneRequest;
    public override void Interact()
    {
        base.Interact();
        HideInteractable();
        GameManager.Instance.CollectItem(this);
        foreach (var item in linkedItems)
        {
            item.HideInteractable();
        }
        if (pin) HidePin(1.5f);
    }

    public override void HideInteractable()
    {
        base.HideInteractable();
        if (pin) HidePin(1.5f);
    }

    protected override void OnStart()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", GameManager.Instance.interactionProperties.itemColor);
        GameManager.Instance.allItems.Add(this);
        HideInteractable();
        if (pin)
        {
            pin.localScale = Vector3.zero;
        }
    }

    public void ShowPin(float duration)
    {
        LeanTween.scale(pin.gameObject, Vector3.one, duration * 0.8f).setEaseInBounce();
    }

    public void HidePin(float duration)
    {
        LeanTween.scale(pin.gameObject, Vector3.zero, duration * 0.8f).setEaseInBounce();
    }

    public override void ShowInteractable()
    {
        base.ShowInteractable();
        if (pin) ShowPin(2f);
    }
}
