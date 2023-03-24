using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneLight : MonoBehaviour
{
    [SerializeField] float moveDelay;
    [SerializeField] BoxCollider patrolBounds;
    [SerializeField] Transform lookAtPosition;
    [SerializeField] Transform idlePosition;
    Vector3 rndPos;
    bool waiting;
    public bool activated;

    // Update is called once per frame
    void Update()
    {
        if (!waiting)
        {
            StartCoroutine("LerpDrone");
        }
        else
        {
            //LeanTween.move(gameObject, rndPos, moveDelay*10f);
            transform.position = Vector3.Lerp(transform.position, rndPos, Time.deltaTime);
        }
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition.position - transform.position);
        transform.rotation = targetRotation;
    }

    IEnumerator LerpDrone()
    {
        waiting = true;
        rndPos = idlePosition.position;
        if (activated) rndPos = RandomPointInBound(patrolBounds.bounds);
        //transform.LookAt(lookAtPosition);
        yield return new WaitForSeconds(moveDelay);
        waiting = false;
    }

    Vector3 RandomPointInBound(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
            );
    }
}
