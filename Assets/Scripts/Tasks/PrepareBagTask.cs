using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareBagTask : Task
{
    public override void Interact()
    {
        base.Interact();
        PorteBB porteBB = (PorteBB)GameManager.Instance.player.equipment;
        BigBagQuest bq = (BigBagQuest)quest;

        if (porteBB.porteBBType == PorteBBType.Crochet)
        {
            bq.bigBag.bigBagAnchor.SetParent(porteBB.bigBagSupport.rotationCrochetPivot);
        }
        else
        {
            bq.bigBag.bigBagAnchor.SetParent(porteBB.bigBagSupport.elevationClassPivot);
        }
        
        porteBB.Use_1();
    }
    
}
