using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="Graphic Settings Data", menuName ="Data/Graphic Settings")]
public class GraphicSettings : ScriptableObject
{
    [Range(0f, 1f)]
    public float grassDensity;
    [Range(0f, 250f)]
    public float grassDistance;

    public UnityAction<float> densityEvent;
    public UnityAction<float> distanceEvent;

    public void UpdateDensity(float to)
    {
        if (densityEvent != null)
        {
            grassDensity = to;
            densityEvent.Invoke(to);
        }
    }

    public void UpdateDistance(float to)
    {
        if (distanceEvent != null)
        {
            grassDistance = to;
            distanceEvent.Invoke(to);
        }
    }
}
