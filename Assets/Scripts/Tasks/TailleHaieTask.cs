using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TailleHaieTask : Task
{
    NavMeshAgent nav;
    [SerializeField] Transform endOfLine;
    PlayerController playerController;
    protected override void OnStart()
    {
        base.OnStart();
        nav = GameManager.Instance.velo.GetComponent<NavMeshAgent>();
        playerController = nav.GetComponent<PlayerController>();
    }
    public override void Interact()
    {
        if (GameManager.Instance.collectedItems.Contains(necessaryItem))
        {
            quest.CompleteTask(this);
        }
        else
        {
            int r = Random.Range(0, 100);
            if (r > sucessChance)
            {
                HandleFailedTask();
                RecapManager.instance.medicalRecap.AddInjurie(Parts.Dos, 1f);
            }
            else
            {
                Debug.Log("Task sucessfully not failed");
            }
            quest.CompleteTask(this);
        }

        if (quest.isFinished() == this)
        {
            necessaryItem.HideInteractable();
        }

        nav.enabled = true;
        playerController.canMove = false;
        nav.transform.LookAt(endOfLine);
        nav.SetDestination(endOfLine.position);
        Debug.Log(nav.pathStatus);
    }

    private void Update()
    {
        if (nav.enabled && Vector3.Distance(nav.transform.position,nav.destination)<= 1f)
        {
            nav.enabled = false;
            playerController.canMove = true;
        }
    }
}
