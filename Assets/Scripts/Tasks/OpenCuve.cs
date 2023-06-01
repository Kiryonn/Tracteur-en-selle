using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCuve : Task
{
    [SerializeField] Animator cuveAnim;
    [SerializeField] GazZone gazZone;

    protected override void OnStart()
    {
        base.OnStart();
    }

    public override void Interact()
    {
        base.Interact();
        gazZone.ActivateZone(10f);
        cuveAnim.SetBool("Open", true);

    }
}
