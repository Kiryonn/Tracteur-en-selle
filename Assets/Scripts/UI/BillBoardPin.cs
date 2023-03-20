using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BillBoardPin : MonoBehaviour
{
    Transform target;
    public enum BillBoardType { LookAtCamera, CameraForward};
    [SerializeField] BillBoardType billBoardType;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.Instance.cam.transform; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (billBoardType)
        {
            case BillBoardType.LookAtCamera:
                transform.LookAt(target.position, Vector3.up);
                break;
            case BillBoardType.CameraForward:
                transform.forward = target.forward;
                break;
            default:
                break;
        }
        
    }
}
