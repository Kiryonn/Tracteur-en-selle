using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TakeBottle : MonoBehaviour
{
    float distanceTraveled;
    public float speed;
    [HideInInspector]
    public float multiplier = 1f;
    public PathCreator pathCreator { get; private set; }

    List<Bottle> bottleList = new List<Bottle>();
    [SerializeField] GameObject bottlePrefab;
    // Start is called before the first frame update
    void Start()
    {
        pathCreator = GetComponent<PathCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = bottleList.Count - 1; i >= 0; --i)
        {
            bottleList[i].distanceTravelled += speed * Time.deltaTime * multiplier;
            bottleList[i].UpdatePosition(pathCreator.path.GetPointAtDistance(bottleList[i].distanceTravelled));
            if (bottleList[i].grabbed)
            {
                bottleList.RemoveAt(i);
            }
            
        }
    }

    public void SpawBottle()
    {
        Bottle bottle = Instantiate(bottlePrefab).GetComponent<Bottle>();
        bottle.transform.position = pathCreator.path.GetPoint(0);
        bottleList.Add(bottle);
    }

    public void AddBottle(Bottle bottle)
    {
        bottle.distanceTravelled = 0f;
        bottle.transform.position = pathCreator.path.GetPoint(0);
        bottleList.Add(bottle);
        
    }
}
