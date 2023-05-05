using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareBagTask : Task
{
    public override void Interact()
    {
        base.Interact();
        PorteBB porteBB = (PorteBB)GameManager.Instance.player.equipment;
        porteBB.Use_1();
    }
    
}
