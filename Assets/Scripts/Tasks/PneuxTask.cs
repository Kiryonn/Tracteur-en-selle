using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PneuxTask : Task
{
    [SerializeField] GameObject objectToRemove;
    [SerializeField] Transform newTransform;
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

        Transform pTransform = GameManager.Instance.player.transform;

        if (newTransform)
        {
            pTransform.position = newTransform.position;
            pTransform.rotation = newTransform.rotation;
        }
        objectToRemove.SetActive(false);
    }
}
