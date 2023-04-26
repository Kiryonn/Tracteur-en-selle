using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageEndTask : Task
{

    int index = 0;

    public override void ShowInteractable()
    {
        if (index == 1)
        {
            base.ShowInteractable();
        }
        else
        {
            index = 1;
        }
        
    }
}
