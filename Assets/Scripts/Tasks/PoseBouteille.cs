using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoseBouteille : TaskPedalez
{
    bool gotTranspalette;
    [SerializeField] bool needTranspalette;
    public bool correctHigh;
    Animator pAnim;
    [SerializeField] Offset playerOffset;
    [SerializeField] BottleStack bottleStack;
    [SerializeField] GameObject fillImageRoot;
    [SerializeField] Image fillImage;
    [SerializeField] TextMeshProUGUI percentText;
    [SerializeField] bool stopGenerator;
    float oldprogress = 0f;
    protected override void OnStart()
    {
        base.OnStart();
        if (!needTranspalette)
        {
            gotTranspalette = true;
        }
        pAnim = GameManager.Instance.player.playerAnim;
        LeanTween.scale(fillImageRoot, Vector3.zero, 1f).setEaseInBounce();
    }

    public override void ShowInteractable()
    {
        if (!gotTranspalette) { return; }
        base.ShowInteractable();

    }

    protected override void ItemCollectedTrigger(Item item)
    {
        if (requiredObjects.Contains(item))
        {
            gotTranspalette = true;
            correctHigh = true;
            ShowInteractable();
        }
    }

    protected override void OnProgessChanged(float p)
    {
        base.OnProgessChanged(p);
        fillImage.fillAmount = p / duration;
        percentText.text = Mathf.RoundToInt(fillImage.fillAmount * 100) + "%";
        if (p <= oldprogress)
        {
            pAnim.SetFloat("AnimSpeed", 0);
        }
        else
        {
            pAnim.SetFloat("AnimSpeed", 1);
        }
        oldprogress = p;
    }

    protected override void HandleBeforePedale()
    {
        playerOffset.SetOffset(GameManager.Instance.player.transform, Space.Self, true);
        fillImage.fillAmount = 0f;
        percentText.text = 0 + "%";
        LeanTween.scale(fillImageRoot, Vector3.one, 1f).setEaseInBounce();
        if (correctHigh)
        {
            pAnim.SetBool("Haut",true);
        }
        else
        {
            RecapManager.instance.medicalRecap.AddInjurie(Parts.Epaule, 1f);
            RecapManager.instance.medicalRecap.AddInjurie(Parts.Dos, 5f);
            GameManager.Instance.SetPenteScaledWithDmg(0.8f);
            pAnim.SetBool("Bas", true);
        }
        pAnim.SetTrigger("PoseBouteille");
    }

    protected override void HandleFinishPedale()
    {
        base.HandleFinishPedale();
        LeanTween.scale(fillImageRoot, Vector3.zero, 1f).setEaseInBounce();
        fillImage.fillAmount = 1f;
        percentText.text = 100 + "%";
        GameManager.Instance.SetPenteScaledWithDmg();
        if (stopGenerator)
        {
            GlobalMachine.instance.StopAllMachines();
            bottleStack.stacks[0].SetActive(false);
        }
        pAnim.SetBool("Haut", false);
        pAnim.SetBool("Bas", false);
    }
}
