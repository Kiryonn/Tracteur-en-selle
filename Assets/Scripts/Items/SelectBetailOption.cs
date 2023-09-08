using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBetailOption : Item
{
    [SerializeField] Offset offset;
    [SerializeField] GameObject obj;

    QuestBuildABetail realQuest;
    bool firstCheck;
    public GameObject objInst { get; private set; }

    protected override void OnStart()
    {
        base.OnStart();

    }

    public override void ShowInteractable()
    {
        base.ShowInteractable();
        realQuest = (QuestBuildABetail)quest;
    }

    public override void Interact()
    {
        base.Interact();
        try
        {
            objInst = Instantiate(obj);
            if (realQuest.lastInstancedObject) { objInst.transform.SetParent(realQuest.lastInstancedObject.transform); }
            offset.SetOffset(objInst.transform);
            StartCoroutine(FadeTextures(5f, 1f));
        }
        catch (System.Exception)
        {

            //throw;
        }

        GameManager.Instance.SwitchCam(CamTypes.Show, null, true, 5f);
    }

    IEnumerator FadeTextures(float duration, float endAlpha, bool delete = false)
    {
        Renderer[] materials = objInst.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].material.SetFloat("_Blend", 0f);
            materials[i].material.SetFloat("_Opacity", 1 - endAlpha);
        }

        for (float j = 0f; j < 1.0f; j += Time.deltaTime / duration)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].material.SetFloat("_Opacity", Mathf.Lerp(1 - endAlpha, endAlpha, j));
            }
            yield return null;
        }

        if (delete)
        {
            Destroy(objInst);
        }
    }

    public void RemoveHologram()
    {
        if (objInst)
        {
            StartCoroutine(FadeTextures(3f, 0f, true));
        }
    }

    public void FadeHologram()
    {
        try
        {
            Renderer[] materials = objInst.GetComponentsInChildren<Renderer>();

            LeanTween.value(gameObject, 0f, 1f, 3f).
                setOnUpdate((float value) =>
                {
                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i].material.SetFloat("_Blend", value);
                    }
                });
        }
        catch (System.Exception)
        {

            MyDebug.Log("No hologram. Ignore if the selection is supposed to be empty");
        }

    }
}
