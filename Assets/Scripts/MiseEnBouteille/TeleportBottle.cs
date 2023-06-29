using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TeleportBottle : MonoBehaviour
{
    [SerializeField] Offset offset;
    [SerializeField] float delay;

    Bottle comp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Bottle>(out comp))
        {
            Invoke("Teleport", delay);
        }
    }

    void Teleport()
    {
        comp.GetComponent<Rigidbody>().isKinematic = true;
        comp.transform.position = offset.position;
        //offset.SetOffset(comp.transform,Space.World,true);
        comp.transform.rotation = Quaternion.Euler(-90f,0f,0f);
    }
}
