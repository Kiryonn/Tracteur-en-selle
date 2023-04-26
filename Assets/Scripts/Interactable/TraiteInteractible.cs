using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraiteInteractible : Interactable
{
    public bool activated { get; private set; }
    [SerializeField] Transform floor;
    [SerializeField] float targetHeight;
    [SerializeField] float speed;
    float starterHeight;

    protected override void OnStart()
    {
        base.OnStart();
        HideInteractable();
        starterHeight = floor.localPosition.y;
        
    }

    public override void Interact()
    {
        base.Interact();
        activated = !activated;
        if (activated)
        {
            LeanTween.moveLocalY(floor.gameObject, starterHeight + targetHeight, speed);
        }
        else
        {
            LeanTween.moveLocalY(floor.gameObject, starterHeight, speed);
        }
        
    }

    
}
