using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Interactable : MonoBehaviour
{
    public string _name;
    protected Renderer render;
    Drone drone;
    private void Start()
    {
        render = GetComponent<Renderer>();
        OnStart();
        drone = GameManager.Instance.drone;
        drone.deliveryEvent.AddListener(ItemDeliveredTrigger);
    }
    public virtual void Interact()
    {
        Debug.Log("Interacting with " + _name);
    }

    public virtual void HideInteractable()
    {
        StartCoroutine(Fade(1.0f, 0f));
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public virtual void ShowInteractable()
    {
        StartCoroutine(Fade(1.0f, 1.0f));
        gameObject.GetComponent<Collider>().enabled = true;
    }
    
    IEnumerator Fade(float aTime, float aValue)
    {
        float a = render.material.GetFloat("_Alpha");
        
        for (float i = 0.0f; i < 1.0f; i +=Time.deltaTime / aTime)
        {
            render.material.SetFloat("_Alpha",Mathf.Lerp(a, aValue, i));
            yield return null;
        }
        render.material.SetFloat("_Alpha", aValue);
    }

    public virtual void ExitInteractable()
    {
        
    }

    protected virtual void OnStart()
    {
        GetComponent<Renderer>().material.SetColor("_Color", GameManager.Instance.interactionProperties.otherColor);
    }

    protected virtual void ItemDeliveredTrigger(Item item)
    {
        //Debug.Log("An item got delivered");
    }
}
