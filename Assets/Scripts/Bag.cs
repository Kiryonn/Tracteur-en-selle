using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public HingeJoint startRope;
    public Transform bigBagAnchor;
    public ParticleSystem seeds;

    private void Start()
    {
        BigBagQuest currentBBQ = (BigBagQuest)GameManager.Instance.currentQuest;
        if (currentBBQ != null)
        {
            currentBBQ.bigBag = this;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Terrain>() != null)
        {
            Debug.Log("I hit the ground");
        }
        Debug.Log("I collided with : " + collision.gameObject.name);
    }
}
