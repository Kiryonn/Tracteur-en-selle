using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBetailOption : Item
{
    [SerializeField] Offset offset;
    [SerializeField] GameObject obj;
    GameObject objInst;
    public override void Interact()
    {
        base.Interact();
        objInst = Instantiate(obj);
        offset.SetOffset(objInst.transform);
        StartCoroutine(FadeTextures(5f));
    }

    IEnumerator FadeTextures(float duration)
    {
        Renderer[] materials = objInst.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].material.SetFloat("_Blend", 0f);
            materials[i].material.SetFloat("_Opacity", 0f);
        }

        for (float j = 0f; j<1.0f; j += Time.deltaTime / duration)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].material.SetFloat("_Opacity", j);
            }
            yield return null;
        }   
    }

    public void FadeHologram()
    {

    }
}
