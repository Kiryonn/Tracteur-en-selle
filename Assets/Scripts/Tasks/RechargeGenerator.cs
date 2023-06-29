using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RechargeGenerator : TaskPedalez
{
    [SerializeField] Image fillBattery;
    [SerializeField] GameObject goodBatImage;
    [SerializeField] GameObject badBatImage;
    protected override void OnStart()
    {
        base.OnStart();
        goodBatImage.SetActive(true);
        badBatImage.SetActive(false);
        fillBattery.fillAmount = 0f;
    }
    public override void ShowInteractable()
    {
        base.ShowInteractable();
        goodBatImage.SetActive(false);
        badBatImage.SetActive(true);
    }
    protected override void OnProgessChanged(float p)
    {
        base.OnProgessChanged(p);
        fillBattery.fillAmount = p/duration;
    }
    protected override void HandleBeforePedale()
    {
        base.HandleBeforePedale();
        
        GameManager.Instance.SwitchCam(CamTypes.Generator);
    }
    protected override void HandleFinishPedale()
    {
        base.HandleFinishPedale();
        fillBattery.fillAmount = 1f;
        goodBatImage.SetActive(true);
        badBatImage.SetActive(false);
        GameManager.Instance.SwitchCam(CamTypes.Character);
        GlobalMachine.instance.StartAllMachines();
    }
}
