using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorSpawner : MonoBehaviour
{
    [SerializeField] List<Transform> forestWaypoints;
    [SerializeField] GameObject raptorPrefab;

    [SerializeField] List<Raptor> raptors;
    void Start()
    {
        raptors = new List<Raptor>();
    }

    public void SpawnRaptors()
    {
        foreach (var item in forestWaypoints)
        {
            Raptor temp = Instantiate(raptorPrefab).GetComponent<Raptor>();
            temp.forests = forestWaypoints;
            temp.InitAgentPosition(item.position);
            raptors.Add(temp);
        }
    }

    
}
