using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraiteTask : Task
{
    [SerializeField] Transform griffeTransform;
    Vector3 positionOffset;
    Quaternion rotationOffset;
    [SerializeField] Vector3 scaleOffset;

    public override void Interact()
    {
        TraiteQuete tq = (TraiteQuete)quest;

        positionOffset = griffeTransform.GetChild(0).transform.position;
        rotationOffset = griffeTransform.GetChild(0).transform.rotation;

        SetPositionAndRotation(griffeTransform, positionOffset, rotationOffset);


        RecapManager.instance.medicalRecap.AddInjurie(Parts.Epaule, 0.5f);
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Main, 0.5f);
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Coude, 0.2f);

        if (tq.levier.activated)
        {
            quest.CompleteTask(this);
        }
        else
        {
            int r = Random.Range(0, 100);
            if (r > sucessChance)
            {
                HandleFailedTask();
            }
            else
            {
                MyDebug.Log("Task sucessfully not failed");
            }
            quest.CompleteTask(this);
        }
    }

    protected override void HandleFailedTask()
    {
        base.HandleFailedTask();
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Dos, 0.5f);
    }

    void SetPositionAndRotation(Transform start, Vector3 position, Quaternion rotation)
    {
        start.position = position;
        start.rotation = rotation;
    }
}
