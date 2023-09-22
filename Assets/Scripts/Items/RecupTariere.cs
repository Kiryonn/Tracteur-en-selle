using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecupTariere : Item
{
    [SerializeField] GameObject worldObject;
    public override void Interact()
    {
        base.Interact();
        ForageQuest forage = (ForageQuest)GameManager.Instance.currentQuest;

        forage.isEquipped = true;

        forage.GetCurrentTask().ShowInteractable();

        worldObject.SetActive(false);
    }
}
