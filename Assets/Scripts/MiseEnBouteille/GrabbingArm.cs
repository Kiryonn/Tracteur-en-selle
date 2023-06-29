using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ArmStates
{
    Waiting,
    Grabbing,
    Grab,
    Retracting,
    Throwing
}

public class GrabbingArm : MonoBehaviour
{
    [Header("Pinces")]
    [SerializeField] Transform pinceDroite;
    [SerializeField] Transform pinceGauche;

    [Header("Bras")]
    [SerializeField] Transform bras;
    public float reactivity;

    [Header("Bottle")]
    [SerializeField] Vector3 bottleOffset;

    [Header("Offsets")]
    [SerializeField] Vector3 waitingPos;
    [SerializeField] Vector3 grabbingPos;

    [HideInInspector]
    public float multiplier = 1f;

    

    ArmStates armStates;

    public void StartArmMovement(Bottle bot)
    {
        float startZ = bras.localPosition.z;
        if (multiplier == 0f)
        {
            return;
        }
        LeanTween.moveLocalZ(bras.gameObject, grabbingPos.z, reactivity * 1/multiplier).setOnComplete(() =>
        {
            if (multiplier == 0f)
            {
                LeanTween.moveLocalZ(bras.gameObject, waitingPos.z, 1f);
                return;
            }
            startZ = bras.localPosition.z;
            bot.grabbed = true;
            bot.transform.parent = transform;
            LeanTween.moveLocalZ(bras.gameObject, waitingPos.z, reactivity * 1/multiplier).setOnComplete(() =>
            {
                bot.transform.parent = null;
                bot.GetComponent<Rigidbody>().isKinematic = false;
                bot.grabbed = false;
            });
        });
    }
}
