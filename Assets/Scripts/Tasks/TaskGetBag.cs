using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGetBag : Task
{
    [SerializeField] Offset offset;
    [SerializeField] GameObject bagPrefab;

    bool canShow;

    public override void ShowInteractable()
    {
        if (canShow)
        {
            base.ShowInteractable();
        }
    }

    public override void Interact()
    {
        base.Interact();
        EquipmentRecup equip = (EquipmentRecup)GameManager.Instance.collectedItems[GameManager.Instance.collectedItems.Count - 1];
        GameObject obj = Instantiate(bagPrefab,equip.equipment.transform);

        offset.SetOffset(obj.transform);
        HingeJoint joint = obj.GetComponent<Bag>().startRope;
        //joint.connectedBody = 
    }

    protected override void ItemCollectedTrigger(Item item)
    {
        Debug.Log("An item as been collected : " + item._name);
        
        EquipmentRecup obj = item as EquipmentRecup;
        Debug.Log("Is is an equipment ? : " + obj != null);
        Debug.Log("Is the quest started ? : " + quest.isStarted);
        if (quest.isStarted && obj != null)
        {
            canShow = true;
            ShowInteractable();
        }
    }
}
