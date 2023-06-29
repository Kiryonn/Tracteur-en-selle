using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BottleDetectorType
{
    Grabbing,
    Filling
}
public class BottleDetector : MonoBehaviour
{
    [SerializeField] GrabbingArm grabbingArm;
    [SerializeField] FillingMachine machineOne;
   
    [SerializeField] BottleDetectorType type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Bottle>(out var comp))
        {
            switch (type)
            {
                case BottleDetectorType.Grabbing:
                    grabbingArm.StartArmMovement(comp);
                    break;
                case BottleDetectorType.Filling:
                    if (comp.bottleState == BottleState.Ready)
                    {
                        comp.bottleState = BottleState.Waiting;
                        machineOne.FinishProcess(comp);
                    }
                    else
                    {
                        machineOne.StartProcess(comp);
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
