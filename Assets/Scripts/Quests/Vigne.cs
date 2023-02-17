using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vigne : Quest
{
    public Secateur secateur;
    public List<MachineSecateur> machines;

    public void SetSecateur(Secateur s)
    {
        secateur = s;
        requiredTasks[0].ShowInteractable();
        foreach (var item in machines)
        {
            item.ShowInteractable();
            item.vigne = this;
        }
    }

    public override void CompleteTask(Task task)
    {
        
        base.CompleteTask(task);
        if (requiredTasks.Count <= 0)
        {
            foreach (var item in machines)
            {
                item.HideInteractable();
            }
        }
    }
}
