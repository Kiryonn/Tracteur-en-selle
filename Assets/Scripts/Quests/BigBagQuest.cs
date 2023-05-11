using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBagQuest : Quest
{
    public Bag bigBag;
    public Offset offsetPlayerEnd;
    public Transform particleColliders;
    public Transform seedPlane;
    [SerializeField] float fillSpeed;
    public bool lostTheBag;
    public void FillSemoir()
    {
        seedPlane.position = new Vector3(seedPlane.position.x, seedPlane.position.y+fillSpeed, seedPlane.position.z);
    }

    public override void CompleteTask(Task task)
    {
        task.HideInteractable();
        requiredTasks.Remove(task);
        if (requiredTasks.Count > 0)
        {
            NextTask();
        }
        else
        {
            if (vision)
            {
                vision.HideInteractable();
            }
            Invoke("DelayComplete", 8f);
        }
    }

    void DelayComplete()
    {
        GameManager.Instance.CompleteQuest(this);
        GameManager.Instance.currentQuest = null;
    }
}
