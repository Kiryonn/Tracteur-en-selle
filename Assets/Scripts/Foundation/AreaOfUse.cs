using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfUse : MonoBehaviour
{
    Interactable interactable;
    public float timeToInteract;
    bool interacted;
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    public IEnumerator EnterArea()
    {
        yield return new WaitForSeconds(timeToInteract);
        interactable.Interact();
        //gameObject.SetActive(false);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !interacted)
        {
            Debug.Log("Entering zone");
            StartCoroutine("EnterArea");
            interacted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && interacted)
        {
            Debug.Log("Exiting zone");
            StopCoroutine("EnterArea");
            interactable.ExitInteractable();
            interacted = false;
        }
    }
}
