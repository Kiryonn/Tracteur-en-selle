using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TrailerChainVFX : MonoBehaviour
{
    [SerializeField] VisualEffect vfxGraph;
    Transform anchorObject;
    Transform trailerObject;

    [SerializeField] Transform startPoint;
    [SerializeField] Transform bezierStart;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform bezierEnd;
    // Start is called before the first frame update
    void Start()
    {
        vfxGraph.Stop();
    }

    public void AttachVFX(Transform anch, Vector3 anchorOffset, Transform trail, Vector3 trailerOffset)
    {
        anchorObject = anch;
        trailerObject = trail;

        startPoint.SetParent(trailerObject);
        endPoint.SetParent(anchorObject);

        startPoint.localPosition = trailerOffset;
        endPoint.localPosition = anchorOffset;

        vfxGraph.Play();
    }

    public void DetachVFX()
    {
        vfxGraph.Stop();
        vfxGraph.enabled = false;
        vfxGraph.enabled = true;
        endPoint.SetParent(vfxGraph.transform.parent);
        startPoint.SetParent(vfxGraph.transform.parent);
    }
}
