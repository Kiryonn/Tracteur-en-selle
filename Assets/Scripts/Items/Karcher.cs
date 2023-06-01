using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karcher : CleaningMaterial
{
    //[SerializeField] GameObject karcherEquip;
    KarcherController karcherController;
    public override void Interact()
    {
        base.Interact();
        karcherController = GameManager.Instance.player.GetComponent<KarcherController>();
        karcherController.GetKarcher();
    }

    public override void Use(NettoyageCuve net)
    {
        base.Use(net);
        karcherController.UseKarcher();
    }
}
