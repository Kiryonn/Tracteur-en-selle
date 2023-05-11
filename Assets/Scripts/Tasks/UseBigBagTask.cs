using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBigBagTask : Task
{
    BigBagQuest bigBagQuest;
    [SerializeField] GameObject specialCam;
    [SerializeField] Offset seedPlaneOffset;

    public override void ShowInteractable()
    {
        base.ShowInteractable();
        gameObject.GetComponent<Collider>().enabled = false;
        Invoke("ShowCollider", 5f);
    }

    void ShowCollider()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public override void Interact()
    {
        base.Interact();
        bigBagQuest = (BigBagQuest)quest;

        bigBagQuest.particleColliders.SetParent(bigBagQuest.bigBag.transform);

        List<Collider> partColl = new List<Collider>(bigBagQuest.particleColliders.GetComponentsInChildren<Collider>());

        for (int i = 0; i<partColl.Count; i++)
        {
            bigBagQuest.bigBag.seeds.trigger.SetCollider(i, partColl[i]);
        }

        bigBagQuest.bigBag.seeds.Play();
        GameManager.Instance.player.canMove = false;
        GameManager.Instance.SwitchCam(CamTypes.Tractor, specialCam);

        if (bigBagQuest.lostTheBag)
        {
            GameManager.Instance.FailTask();
        }
        Invoke("StopSeeds", 5f);
    }

    void StopSeeds()
    {
        bigBagQuest.bigBag.seeds.Stop();
        GameManager.Instance.player.canMove = true;
        StartCoroutine(GameManager.Instance.player.SwitchControls("Tractor",true,false));
        Invoke("OffsetPlayer", 3f);
        Destroy(GameManager.Instance.player.equipment.equipment);
    }

    void OffsetPlayer()
    {
        GameManager.Instance.player.transform.localPosition = bigBagQuest.offsetPlayerEnd.position;
        GameManager.Instance.player.transform.localRotation = Quaternion.Euler(bigBagQuest.offsetPlayerEnd.rotation);
    }
}
