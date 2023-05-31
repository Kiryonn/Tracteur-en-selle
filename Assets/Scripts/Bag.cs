using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public HingeJoint startRope;
    public Transform bigBagAnchor;
    public ParticleSystem seeds;
    [SerializeField] float minFloorDistance = 1.5f;
    [SerializeField] GameObject recupBagInteractable;
    bool onTheGround;
    [SerializeField] GameObject obj;


    private void Start()
    {
        BigBagQuest currentBBQ = (BigBagQuest)GameManager.Instance.currentQuest;
        if (currentBBQ != null)
        {
            currentBBQ.bigBag = this;
        }
    }

    private void Update()
    {
        //Debug.Log("Layermask is : " + LayerMask.LayerToName(layerMask));
        RaycastHit hit;
        if (Physics.Raycast(bigBagAnchor.transform.position, transform.TransformDirection(Vector3.down), out hit))
        {
            Debug.DrawRay(bigBagAnchor.transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            
            if (hit.distance < minFloorDistance && !onTheGround)
            {
                Debug.Log("Raycast hit : "+hit.collider.gameObject.name);
                onTheGround = true;
                obj = Instantiate(recupBagInteractable);
                obj.transform.position = new Vector3(transform.position.x,obj.transform.position.y,transform.position.z);
                obj.GetComponent<RecupBag>().bag = this;
                GameManager.Instance.currentQuest.GetCurrentTask().HideInteractable();
            }
        }
    }
}
