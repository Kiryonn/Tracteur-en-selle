using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canne : CleaningMaterial
{
    [SerializeField] GameObject deployedCanne;
    [SerializeField] float animDuration = 3f;
    public override void Use(NettoyageCuve net)
    {
        GameManager.Instance.player.playerAnim.SetTrigger("Use");
        StartCoroutine(UsingAnim(animDuration, net));
    }

    IEnumerator UsingAnim(float wait, NettoyageCuve n)
    {
        yield return new WaitForSeconds(wait);
        deployedCanne.SetActive(true);
        used = true;
        n.Interact();
    }
}
