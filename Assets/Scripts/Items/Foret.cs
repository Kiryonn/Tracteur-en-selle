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
    Quaternion previousQuat;

    [Header("Shaki shaki")]
    Vector3 startPos;
    [SerializeField] float shakeSpeed;
    [SerializeField] float shakeAmount;
    Transform playerTransform;
    Vector3 startPlayerPos;

    [Header("VFX")]

    [SerializeField] GameObject dirtVFX;
    bool vfxIsPlaying = true;

    [Header("SFX")]
    LoopingSound loopingSound;
    void Start()
    {
        anim = GetComponent<Animator>();
        p = GameManager.Instance.player;
        startQuat = pic.localRotation;
        previousQuat = pic.localRotation;

        startPos = transform.localPosition;
        playerTransform = GameManager.Instance.player.charaGraphics.transform;
        startPlayerPos = playerTransform.localPosition;

        loopingSound = GetComponent<LoopingSound>();

        ToggleVFX(false);
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
            ToggleVFX(true);
            pic.localRotation = previousQuat;
            pic.Rotate(Vector3.down * rotationSpeed, Space.World);
            previousQuat = pic.localRotation;

        }
        else
        {
            ToggleVFX(false);
            pic.localRotation = startQuat;
            previousQuat = startQuat;
        }

        transform.localPosition = new Vector3(startPos.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount,
                                              startPos.y + Mathf.Sin(Time.deltaTime * shakeSpeed) * shakeAmount,
                                              startPos.z + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);

        playerTransform.localPosition = new Vector3(startPlayerPos.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount,
                                                    startPlayerPos.y + Mathf.Sin(Time.deltaTime * shakeSpeed) * shakeAmount,
                                                    startPlayerPos.z + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);


    }

    void ToggleVFX(bool onOff)
    {
        if (onOff && !vfxIsPlaying)
        {
            foreach (var item in dirtVFX.GetComponentsInChildren<ParticleSystem>())
            {
                item.Play();
            }

            vfxIsPlaying = true;
            loopingSound.PlayFromStart(0, true);
        }
        else if (!onOff && vfxIsPlaying)
        {
            foreach (var item in dirtVFX.GetComponentsInChildren<ParticleSystem>())
            {
                item.Stop();
            }
            loopingSound.Stop(false);
            vfxIsPlaying = false;
        }
    }

}
