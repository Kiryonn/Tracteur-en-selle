using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canne : CleaningMaterial
{
    [SerializeField] GameObject deployedCanne;
    [SerializeField] GameObject unDeployedCanne;
    [SerializeField] float animDuration = 3f;
    [SerializeField] GameObject VFXSmokeEffect;
    [SerializeField] Transform smokePosition;

    protected override void OnStart()
    {
        base.OnStart();
        deployedCanne.SetActive(false);
    }
    public override void Interact()
    {
        base.Interact();
        unDeployedCanne.SetActive(false);
    }
    public override void Use(NettoyageCuve net)
    {
        GameManager.Instance.player.playerAnim.SetTrigger("Use");
        StartCoroutine(UsingAnim(animDuration, net));
       
    }

    IEnumerator UsingAnim(float wait, NettoyageCuve n)
    {
        yield return new WaitForSeconds(wait);
        Destroy(Instantiate(VFXSmokeEffect, smokePosition),10f);
        deployedCanne.SetActive(true);
        used = true;
        n.Interact();
    }
}
