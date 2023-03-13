using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BillBoardPin : MonoBehaviour
{
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.Instance.cam.transform; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
