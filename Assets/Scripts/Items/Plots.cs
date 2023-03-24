using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plots : Item
{
    [SerializeField] Transform[] plotsPosition;
    [SerializeField] GameObject plotPrefab;
    Transform[] plots;

    protected override void OnStart()
    {
        base.OnStart();
        plots = new Transform[plotsPosition.Length];
    }

    public override void Interact()
    {
        base.Interact();
        for (int i = 0; i<plotsPosition.Length; i++)
        {
            Debug.Log("i = " + i);
            plots[i] = Instantiate(plotPrefab).transform;
            plots[i].position = plotsPosition[i].position;
        }
    }

    public void RemovePlots()
    {
        for (int i = 0; i<plots.Length; i++)
        {
            Destroy(plots[i].gameObject,1.5f);
        }
    }
}
