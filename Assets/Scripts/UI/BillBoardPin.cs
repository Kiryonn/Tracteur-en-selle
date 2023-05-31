using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BillBoardPin : MonoBehaviour
{
    Transform target;
    [SerializeField] float minimalDistance = 0f; // 0 is infinite
    public enum BillBoardType { LookAtCamera, CameraForward};
    [SerializeField] BillBoardType billBoardType;
    Quaternion startRot;
    Transform playerObj;
    [SerializeField] float smoothLookAt = 50f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.Instance.cam.transform;
        startRot = transform.rotation;
        playerObj = GameManager.Instance.player.transform;
    }
    bool PlayerInRange()
    {
        float dist = Vector3.Distance(transform.position, playerObj.position);
        return dist < minimalDistance;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (minimalDistance == 0f || PlayerInRange())
        {
            switch (billBoardType)
            {
                case BillBoardType.LookAtCamera:
                    Vector3 dir = target.position - transform.position;
                    //transform.LookAt(target.position, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir),Time.deltaTime * smoothLookAt);
                    break;
                case BillBoardType.CameraForward:
                    transform.forward = target.forward;
                    break;
                default:
                    break;
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startRot, Time.deltaTime * smoothLookAt);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, minimalDistance);
    }
}
