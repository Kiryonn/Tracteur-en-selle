using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfUse : MonoBehaviour
{
    Interactable interactable;
    public float timeToInteract;
    Material material;
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        interactable = GetComponent<Interactable>();
        if (interactable.customColorization)
        {
            material.SetColor("_Color", interactable.customColor);
        }
    }

    public IEnumerator EnterArea()
    {
        yield return new WaitForSeconds(timeToInteract);
        interactable.Interact();
        gameObject.SetActive(false);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Entering zone");
            StartCoroutine("EnterArea");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Exiting zone");
            StopCoroutine("EnterArea");
        }
    }
}
