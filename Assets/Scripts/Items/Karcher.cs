using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karcher : CleaningMaterial
{
    [SerializeField] GameObject karcherUndeployed;
    KarcherController karcherController;
    [SerializeField] Offset playerOffset;
    public override void Interact()
    {
        base.Interact();
        
        karcherController = GameManager.Instance.player.GetComponent<KarcherController>();
        karcherController.GetKarcher(karcherUndeployed);
    }

    public override void Use(NettoyageCuve net)
    {
        //base.Use(net);
        GameManager.Instance.player.canMove = false;
        playerOffset.SetOffset(GameManager.Instance.player.transform,Space.Self,true);
        karcherController.UseKarcher();
        StartCoroutine(UsingAnim(5f, net));
    }
    IEnumerator UsingAnim(float wait, NettoyageCuve n)
    {
        yield return new WaitForSeconds(wait);
        GameManager.Instance.player.canMove = true;
        used = true;
        n.Interact();
    }
}
