using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSecateur : Interactable
{
    public Vigne vigne;
    public bool affutage;

    protected override void OnStart()
    {
        base.OnStart();
        HideInteractable();
    }

    public override void Interact()
    {
        base.Interact();
        Use();
    }

    public void Use()
    {
        if (affutage)
        {
            vigne.secateur.Affutage();
            vigne.secateur.affutage = false;
            Debug.Log("Affutage du secateur " + vigne.secateur.type.ToString());
        }
        else
        {
            vigne.secateur.Affilage();
            Debug.Log("Affilage du secateur " + vigne.secateur.type.ToString());
        }
    }
}
