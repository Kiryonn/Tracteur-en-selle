using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PneuxTask : Task
{
    [SerializeField] GameObject objectToRemove;
    [SerializeField] bool switchToTractor;

    public override void Interact()
    {
        base.Interact();
        if (switchToTractor)
        {
            StartCoroutine(GameManager.Instance.player.SwitchControls("Tractor", true));
            Invoke("RemoveObj", 3f);
        }
        else
        {
            RemoveObj();
        }
        
        
    }

    void RemoveObj()
    {
        objectToRemove.SetActive(false);
    }
}
