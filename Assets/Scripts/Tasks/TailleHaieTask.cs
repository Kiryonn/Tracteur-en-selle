using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TailleHaieTask : Task
{
    NavMeshAgent nav;
    [SerializeField] Transform endOfLine;
    PlayerController playerController;
    bool destinationReached;
    protected override void OnStart()
    {
        base.OnStart();
        nav = GameManager.Instance.velo.GetComponent<NavMeshAgent>();
        playerController = nav.GetComponent<PlayerController>();
    }
    public override void Interact()
    {
        

        /*nav.enabled = true;
        playerController.canMove = false;
        nav.transform.LookAt(endOfLine);
        nav.SetDestination(endOfLine.position);
        Debug.Log(nav.pathStatus);*/
        playerController.ForceDestination(transform.position,endOfLine.position,1f);
        StartCoroutine("WaitForDestination");
    }

    IEnumerator WaitForDestination()
    {
        while (playerController.navState == PlayerController.NavState.Forced)
        {
            yield return null;
        }
        
        Complete();
    }

    void Complete()
    {
        if (CheckNecessaryItem())
        {
            quest.CompleteTask(this);
        }
        else
        {
            int r = Random.Range(0, 100);
            if (r > sucessChance)
            {
                HandleFailedTask();
                RecapManager.instance.medicalRecap.AddInjurie(Parts.Torse, 2f);
                RecapManager.instance.medicalRecap.AddInjurie(Parts.Tete, 1f);
            }
            else
            {
                Debug.Log("Task sucessfully not failed");
            }
            quest.CompleteTask(this);
        }

        if (quest.isFinished() == this)
        {
            HideAllNecessaryItems();
            
        }

    }
}
