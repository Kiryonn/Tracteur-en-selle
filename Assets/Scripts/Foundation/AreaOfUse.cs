using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaOfUse : MonoBehaviour
{
    Interactable interactable;
    public float timeToInteract;
    bool interacted;
    [System.NonSerialized] public UnityEvent<float> onProgressChanged = new UnityEvent<float>();
    [SerializeField] bool dontShowProgressBar;
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    public IEnumerator EnterArea()
    {
        for(float i = 0; i < 1f; i+=Time.deltaTime/timeToInteract)
        {
            onProgressChanged.Invoke(i);
            yield return null;
        }
        onProgressChanged.Invoke(1f);
        if (!dontShowProgressBar) UIManager.instance.RemoveProgressListener(this);
        interactable.Interact();
        interacted = false;
        //gameObject.SetActive(false);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !interacted)
        {
            //MyDebug.Log("Entering zone");
            if (!dontShowProgressBar) UIManager.instance.SetProgressListener(this);
            
            StartCoroutine("EnterArea");
            interacted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && interacted)
        {
            //MyDebug.Log("Exiting zone");
            if (!dontShowProgressBar) UIManager.instance.RemoveProgressListener(this);
            StopCoroutine("EnterArea");
            onProgressChanged.Invoke(0f);
            interactable.ExitInteractable();
            interacted = false;
        }
    }
}
