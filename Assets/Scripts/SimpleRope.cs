using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRope : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;

    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0,startPoint.position);
        lineRenderer.SetPosition(1,endPoint.position);
    }
}
