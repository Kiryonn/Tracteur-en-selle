using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plots : Item
{
    [SerializeField] Transform[] plotsPosition;
    [SerializeField] GameObject plotPrefab;
    Transform[] plots;

    public override void Interact()
    {
        base.Interact();
        for (int i = 0; i<plotsPosition.Length; i++)
        {
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
