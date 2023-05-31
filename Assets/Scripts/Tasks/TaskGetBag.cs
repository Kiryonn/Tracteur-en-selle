using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGetBag : Task
{

    [SerializeField] GameObject graphic;

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
        PorteBB equip = (PorteBB)GameManager.Instance.player.equipment;
        GameObject obj = Instantiate(equip.bagPrefab,equip.equipment.transform);

        equip.bagOffset.SetOffset(obj.transform);
        Bag b = obj.GetComponent<Bag>();
        BigBagQuest bq = (BigBagQuest)quest;
        bq.bigBag = b;
        if (equip.porteBBType == PorteBBType.Crochet)
        {
            b.bigBagAnchor.SetParent(equip.bigBagSupport.rotationCrochetPivot);
        }
        else
        {
            b.bigBagAnchor.SetParent(null);
        }
        
        //graphic.SetActive(false);
        //joint.connectedBody = 
    }

    protected override void ItemCollectedTrigger(Item item)
    {
        EquipmentRecup obj = item as EquipmentRecup;
        if (quest)
        {
            if (quest.isStarted && obj != null)
            {
                canShow = true;
                ShowInteractable();
            }
        }
    }
}
