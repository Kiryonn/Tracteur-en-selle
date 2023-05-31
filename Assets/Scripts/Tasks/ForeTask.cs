using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeTask : Task
{
    [SerializeField] GameObject dirtPrefab;
    Offset dirtOffset;
    public override void Interact()
    {
        base.Interact();
        ForageQuest forage = (ForageQuest)quest;
        dirtOffset = new Offset(forage.foret.dirtPosition);
        GameObject dirt = Instantiate(dirtPrefab);
        dirtOffset.SetOffset(dirt.transform, Space.World);
        forage.foret.isForage = true;
        GameManager.Instance.player.canMove = false;
        LeanTween.scale(dirt, Vector3.one * 37f, 5f).setOnComplete(() =>
        {
            forage.foret.isForage = false;
            GameManager.Instance.player.canMove = true;
        });
    }
}
