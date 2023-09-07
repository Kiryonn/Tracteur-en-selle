using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeTask : TaskPedalez
{
    [SerializeField] GameObject dirtPrefab;
    [SerializeField] float holeSize = 37f;
    [SerializeField] bool lastFore;
    ForageQuest forage;
    GameObject dirt;

    Offset dirtOffset;

    float oldProgress;

    protected override void HandleBeforePedale()
    {
        base.HandleBeforePedale();

        forage = (ForageQuest)quest;
        if (forage.foret)
        {
            dirtOffset = new Offset(forage.foret.dirtPosition);
        }
        else
        {
            dirtOffset = new Offset(transform);
            GameManager.Instance.velo.ChangePente(45);
        }
        

        dirt = Instantiate(dirtPrefab);

        dirtOffset.SetOffset(dirt.transform, Space.World);

        ForageQuest q = (ForageQuest)quest;

        q.holesPositions.Enqueue(dirt.transform);

        Debug.Log("Tried to enqueue an item, there is " + q.holesPositions.Count + " amount of item inside queue");
    }

    protected override void HandleFinishPedale()
    {
        base.HandleFinishPedale();

        if (forage.foret)
        {
            forage.foret.isForage = false;
        }

        if (lastFore)
        {
            Plots pl;
            GameManager.Instance.player.SetEquipment(null, false);
            GameManager.Instance.SetPenteScaledWithDmg();
            foreach (var item in necessaryItem)
            {
                if (pl = item.gameObject.GetComponent<Plots>())
                {
                    pl.RemovePlots();
                }
            }
        }else if (!forage.foret)
        {
            GameManager.Instance.velo.ChangePente(45);
        }

        dirt.transform.parent = transform;
    }

    protected override void OnProgessChanged(float p)
    {

        if (oldProgress != p && forage.foret)
        {
            forage.foret.isForage = true;
        }
        else if (forage.foret)
        {
            forage.foret.isForage = false;
        }

        float vecParam = Mathf.Lerp(0, holeSize, p / duration);

        dirt.transform.localScale = Vector3.one * vecParam;

        oldProgress = p;

    }
}
