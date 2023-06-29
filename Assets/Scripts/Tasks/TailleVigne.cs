using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailleVigne : Task
{
    public int vigneResistance;
    Vigne v;
    protected override void OnStart()
    {
        base.OnStart();
        GameManager.Instance.remainingTasks.Add(this);
        render.material.SetColor("_Color", GameManager.Instance.interactionProperties.taskColor);
        HideInteractable();
    }
    public override void Interact()
    {
        Vigne v = (Vigne)quest;
        bool b = v.secateur.currentDurability < 5;
        Debug.Log("Secating !!!");
        v.secateur.Use(vigneResistance);
        if (b)
        {
            HandleFailedTask();
        }
        else
        {
            HandleSucceededTask();
        }
    }

    public override void ShowInteractable()
    {
        Vigne v = (Vigne)GameManager.Instance.currentQuest;
        
        if (v.secateur)
        {
            base.ShowInteractable();
        }
        else
        {
            StopAllCoroutines();
            HideInteractable();
        }
        /*
        10 000 / 7
        2000
        5000

        10 000 coups dans une journée
        au bout de 2000 coups -> affilage
        au bout de 5000 coups -> affûtage
        10 coups de sécateur par pied
        1000 pieds par jour
        */
    }

    
}
