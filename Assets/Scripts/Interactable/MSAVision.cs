using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSAVision : Interactable
{
    Camera cam;

    [SerializeField] Transform[] path;
    Transform startView;

    [SerializeField] float transitionTime;
    [SerializeField] float visionLength;

    protected override void OnStart()
    {
        base.OnStart();
        cam = Camera.main;
        
    }

    public override void Interact()
    {
        base.Interact();

    }
}
