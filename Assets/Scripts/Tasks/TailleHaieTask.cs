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
    [SerializeField] float timeOut = 10f;
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
        if (!destinationReached)
        {
            playerController.ForceDestination(transform.position, endOfLine.position, 1f);
            StartCoroutine("WaitForDestination");
        }
        else
        {
            base.Interact();
        }
        
    }

    IEnumerator WaitForDestination()
    {
        float time = 0f;
        while (playerController.navState == PlayerController.NavState.Forced && time<timeOut)
        {
            time += Time.deltaTime;
            yield return null;
        }

        destinationReached = true;
        Interact();
    }

    protected override void HandleFailedTask()
    {
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Torse, 2f);
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Tete, 1f);
        base.HandleFailedTask();
    }

}
