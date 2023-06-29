using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transpalette : Item
{
    [SerializeField] protected GameObject undeployedTranspalette;
    [SerializeField] protected GameObject deployedPalette;
    [SerializeField] protected GameObject deployedTranspalette;
    [SerializeField] protected GameObject movingPart;

    [SerializeField] GameObject smokeVFX;
    [SerializeField] Transform[] smokePosition;

    [SerializeField] float height;

    protected override void OnStart()
    {
        base.OnStart();
        undeployedTranspalette.SetActive(true);
        deployedPalette.SetActive(false);
        deployedTranspalette.SetActive(false);
    }

    public override void Interact()
    {
        base.Interact();
        MiseEnBouteilleQuest m = (MiseEnBouteilleQuest)GameManager.Instance.currentQuest;
        m.currentTranspalette = this;
        for (int i = 0; i < smokePosition.Length; i++)
        {
            Destroy(Instantiate(smokeVFX, smokePosition[i]),10f);
        }
        undeployedTranspalette.SetActive(false);
        deployedPalette.SetActive(true);
        deployedTranspalette.SetActive(true);
    }
    public virtual void Use()
    {
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Epaule, 1f);
        deployedPalette.transform.parent = movingPart.transform;
        LeanTween.moveLocalY(movingPart, movingPart.transform.localPosition.y + height, 2f);
    }
}
