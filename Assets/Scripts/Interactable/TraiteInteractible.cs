using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraiteInteractible : Interactable
{
    public bool activated { get; private set; }

    protected override void OnStart()
    {
        base.OnStart();
        HideInteractable();
    }

    public override void Interact()
    {
        base.Interact();
        activated = !activated;
    }

    
}
