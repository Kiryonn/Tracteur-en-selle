using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO2Detector : Item
{
    [SerializeField] GameObject co2Object;
    [SerializeField] GameObject uiDetector;
    [SerializeField] GazZone gazZone;
    public override void Interact()
    {
        base.Interact();
        co2Object.SetActive(false);
        gazZone.co2UI = GameManager.Instance.UISpawnObject(uiDetector).GetComponent<CO2UI>();
        gazZone.co2UI.activated = true;
    }
}
