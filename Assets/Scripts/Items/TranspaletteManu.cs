using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranspaletteManu : Transpalette
{
    [SerializeField] GameObject levier;
    public override void Interact()
    {
        base.Interact();
        
    }
    public override void Use()
    {
        base.Use();
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Epaule, 1f);
        LeanTween.rotateLocal(levier, new Vector3(-30, 0, 0), 0.5f).setOnComplete(() =>
        {
            LeanTween.rotateLocal(levier, new Vector3(-90, 0, 0), 0.5f).setOnComplete(() =>
            {
                LeanTween.rotateLocal(levier, new Vector3(-30, 0, 0), 0.5f).setOnComplete(() =>
                {
                    LeanTween.rotateLocal(levier, new Vector3(-90, 0, 0), 0.5f);
                });
            });
        });
    }

}
