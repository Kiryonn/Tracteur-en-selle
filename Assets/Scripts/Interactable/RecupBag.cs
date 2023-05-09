using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecupBag : Interactable
{
    public Bag bag;
    public override void Interact()
    {
        base.Interact();
        Destroy(bag.gameObject);
        PorteBB equip = (PorteBB)GameManager.Instance.player.equipment;
        GameObject obj = Instantiate(equip.bagPrefab, equip.equipment.transform);

        equip.bagOffset.SetOffset(obj.transform);
        Bag b = obj.GetComponent<Bag>();
        BigBagQuest bq = (BigBagQuest)GameManager.Instance.currentQuest;
        bq.bigBag = b;
        if (equip.porteBBType == PorteBBType.Crochet)
        {
            b.bigBagAnchor.SetParent(equip.bigBagSupport.rotationCrochetPivot);
        }
        else
        {
            b.bigBagAnchor.SetParent(null);
        }
        HideInteractable();
        Destroy(gameObject, 3f);
        GameManager.Instance.currentQuest.GetCurrentTask().ShowInteractable();
    }
}
