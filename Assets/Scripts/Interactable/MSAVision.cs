using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSAVision : Interactable
{
    Camera cam;

    List<Item> objectsToSee;
    [SerializeField] Vector3 playerOffset;

    [SerializeField] float transitionTime;
    [SerializeField] float visionLength;
    [SerializeField] float height;
    [SerializeField] float lookTransitionTime;
    Vector3 startPos;
    Quaternion startRot;

    LTSpline visualisePath;

    protected override void OnStart()
    {
        base.OnStart();
        cam = Camera.main;
    }

    public override void Interact()
    {
        base.Interact();
        startPos = cam.transform.position;
        startRot = cam.transform.rotation;
        if (!GameManager.Instance.currentQuest)
        {
            return;
        }
        objectsToSee = GameManager.Instance.currentQuest.GetCurrentTask().requiredObjects;

        Vector3 down = Vector3.right * 90f;
        cam.GetComponent<CinemachineBrain>().enabled = false;

        LeanTween.move(cam.gameObject, GameManager.Instance.velo.transform.position + playerOffset, 2f);
        LeanTween.rotate(cam.gameObject, down, 2f).setOnComplete(Elevate);
    }
    void Elevate()
    {
        LeanTween.moveY(cam.gameObject, height, transitionTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(LookAtObject);
    }
    void LookAtObject()
    {
        Vector3 directionToLook = (FindTargetPoint(objectsToSee) - cam.transform.position).normalized;
        Vector3 targetRotation = Quaternion.LookRotation(directionToLook).eulerAngles;
        LeanTween.rotate(cam.gameObject, targetRotation, lookTransitionTime).setEase(LeanTweenType.easeInOutQuad);
        Invoke("ReturnToStart", 3f + visionLength);
    }
    void ReturnToStart()
    {
        Debug.Log("returning to the base : " + startPos);
        LeanTween.move(cam.gameObject, startPos, transitionTime).setEase(LeanTweenType.easeOutQuad);
        LeanTween.rotate(cam.gameObject, startRot.eulerAngles, transitionTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(ResetCam);
    }
    void ResetCam()
    {
        cam.GetComponent<CinemachineBrain>().enabled = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (visualisePath != null)
        {
            visualisePath.gizmoDraw();
        }
    }

    Vector3 FindTargetPoint(List<Item> transforms)
    {
        Vector3 totalVector = Vector3.zero;
        foreach (var item in transforms)
        {
            totalVector += item.transform.position;
        }
        Vector3 centerPoint = totalVector / transforms.Count;
        return centerPoint;
    }
}
