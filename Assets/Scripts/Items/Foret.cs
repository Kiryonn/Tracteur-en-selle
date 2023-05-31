using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foret : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform pic;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform[] wheels;
    [SerializeField] float wheelSpeedMultiplier = 5f;
    [SerializeField] Renderer trackRender;
    public bool isForage;
    public Transform dirtPosition;
    PlayerController p;
    Quaternion startQuat;
    void Start()
    {
        anim = GetComponent<Animator>();
        p = GameManager.Instance.player;
        startQuat = pic.localRotation;

    }
    void LateUpdate()
    {
        if (p != null)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].Rotate(new Vector3(p.movement * wheelSpeedMultiplier, 0, 0), Space.Self);
            }
            float d = Mathf.Max(Mathf.Abs(p.movement), Mathf.Abs(p.rotation));
            trackRender.material.SetFloat("_Speed", d);
        }
        else
        {
            p = GameManager.Instance.player;
        }
        anim.SetBool("Forage", isForage);
        if (isForage)
        {
            pic.localRotation = startQuat;
            pic.Rotate(Vector3.down * rotationSpeed, Space.World);
            startQuat = pic.localRotation;
            
        }
    }

}
