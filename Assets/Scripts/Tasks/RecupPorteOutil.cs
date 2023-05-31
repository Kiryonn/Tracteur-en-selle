using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecupPorteOutil : Task
{
    [SerializeField] GameObject porteOutil;
    [SerializeField] Offset offsetPorteOutil;

    public override void Interact()
    {
        base.Interact();
        if (switchPlayer)
        {
            StartCoroutine(GameManager.Instance.player.SwitchControls(targetChange, true));
        }
        GameObject obj = Instantiate(porteOutil);
        obj.transform.parent = GameManager.Instance.player.transform;
        offsetPorteOutil.SetOffset(obj.transform);
        ForageQuest forage = (ForageQuest)quest;
        forage.foret = obj.GetComponent<Foret>();
    }
}
