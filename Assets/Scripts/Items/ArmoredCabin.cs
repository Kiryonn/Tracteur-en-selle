using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredCabin : Item
{
    Renderer cabinRenderer;
    float blend;

    protected override void OnStart()
    {
        blend = 0;
        base.OnStart();
        cabinRenderer = GameManager.Instance.player.cabin;
    }

    protected override void ItemDeliveredTrigger(Item item)
    {
        base.ItemDeliveredTrigger(item);
        if (item == this)
        {
            LeanTween.value(blend, 1f, 3f)
            .setOnUpdate((float val) =>
            {
                cabinRenderer.material.SetFloat("_ArmorValue", val);
            });
        }
        
    }

}
