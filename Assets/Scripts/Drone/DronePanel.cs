using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePanel : MonoBehaviour
{
    float startY;
    [SerializeField] List<Interactable> requiredInteractions = new List<Interactable>();
    [SerializeField] List<Interactable> lastInteractions = new List<Interactable>(); // drone dissapear if all the interactions have been used. 
                                                                                    // leave empty to always maintain the drone
    bool deactivated;
    float randomTimer;
    // Start is called before the first frame update
    void Start()
    {

        startY = transform.localPosition.y;
        if (requiredInteractions.Count > 0)
        {
            gameObject.SetActive(false);
            deactivated = true;
        }
        foreach (var interactable in requiredInteractions)
        {
            interactable.OnInteract.AddListener(UpdateRequiredInteractions);
        }

        foreach (var interactable in lastInteractions)
        {
            interactable.OnInteract.AddListener(UpdateLastInteractions);
        }

        MoveUp();
    }

    void UpdateRequiredInteractions(Interactable inte)
    {
        requiredInteractions.Remove(inte);
        if (requiredInteractions.Count == 0)
        {
            Appear();
        }
    }

    void UpdateLastInteractions(Interactable inte)
    {
        lastInteractions.Remove(inte);
        if (lastInteractions.Count == 0)
        {
            Dissapear();
        }
    }

    void Appear()
    {
        transform.localPosition += Vector3.up * 10f;
        gameObject.SetActive(true);
        LeanTween.moveLocalY(gameObject, startY, 5f).setEaseInOutBack().setOnComplete(() =>
        {
            deactivated = false;
        });
    }

    void Dissapear()
    {
        LeanTween.moveLocalY(gameObject, startY + 10f, 5f).setEaseInOutBack().setOnComplete(() =>
        {
            gameObject.SetActive(false);
            deactivated = true;
        });
    }

    void MoveUp()
    {
        if (deactivated) { return; }
        randomTimer = Random.Range(5f, 9f);
        LeanTween.moveLocalY(gameObject, startY + 1f, randomTimer).setEaseInOutBack().setOnComplete(MoveDown);
    }

    void MoveDown()
    {
        if (deactivated) { return; }
        randomTimer = Random.Range(5f, 9f);
        LeanTween.moveLocalY(gameObject, startY - 0.5f, randomTimer).setEaseInOutBack().setOnComplete(MoveUp);
    }
}
