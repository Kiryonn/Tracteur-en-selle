using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageTask : Task
{
    bool check;
    DamageController damageController;
    [SerializeField] bool switchToTractor;
    [SerializeField] GameObject tractor;
    protected override void OnStart()
    {
        base.OnStart();
        GameManager.Instance.onCollectedItem.AddListener(CheckItem);
        damageController = GameManager.Instance.velo.GetComponent<DamageController>();
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
        if (switchToTractor)
        {
            StartCoroutine(GameManager.Instance.player.SwitchControls("Tractor", true));
            Invoke("HideTractor", 3f);
        }
        damageController.HealTractor(
            damageController.maxHealth * 0.2f);
    }

    void HideTractor()
    {
        tractor.SetActive(false);
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
