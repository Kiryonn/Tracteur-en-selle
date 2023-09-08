using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plots : Item
{
    [SerializeField] Transform[] plotsPosition;
    [SerializeField] GameObject plotPrefab;
    Transform[] plots;
    bool posed;

    protected override void OnStart()
    {
        base.OnStart();
        plots = new Transform[plotsPosition.Length];
    }

    public override void Interact()
    {
        base.Interact();
        posed = true;
        for (int i = 0; i<plotsPosition.Length; i++)
        {
            MyDebug.Log("i = " + i);
            plots[i] = Instantiate(plotPrefab).transform;
            plots[i].position = plotsPosition[i].position;
        }
    }

    public void RemovePlots()
    {
        for (int i = 0; i<plots.Length; i++)
        {
            if (plots[i])
            {
                Destroy(plots[i].gameObject, 1.5f);
            }
        }
    }

    public override void ShowInteractable()
    {
        if (!posed)
        {
            base.ShowInteractable();
        }
    }
}
