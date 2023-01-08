using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : Interactable
{
    public bool requireItem;
    [HideInInspector]
    public List<Item> requiredObjects;
    [HideInInspector]
    public float sucessChance;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.remainingTasks.Add(this);
    }

    public override void Interact()
    {
        base.Interact();
        if (requireItem)
        {

        }
        GameManager.Instance.CompleteTask(this);
    }
}
