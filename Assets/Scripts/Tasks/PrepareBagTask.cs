using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareBagTask : Task
{
    [SerializeField] float delay;
    int state = 0;
    public override void Interact()
    {
        if (state == 1)
        {
            base.Interact();
            MyDebug.Log("we interact");
            state = 2;
        }
        else
        {
            HideInteractable();
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
            state = 1;
            Invoke("Interact", delay);
        }
        
    }
    
}
