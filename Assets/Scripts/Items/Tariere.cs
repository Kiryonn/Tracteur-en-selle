using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tariere : MonoBehaviour
{
    public bool isDrilling;
    [SerializeField] Transform lame;
    [SerializeField] float rotationSpeed;

    [SerializeField] GameObject dirtVFX;
    bool vfxIsPlaying = true;

    public float startHeight { get; private set; }

    public float endHeigth;

    [Header("SFX")]
    LoopingSound loopingSound;
    // Start is called before the first frame update
    void Start()
    {
        loopingSound = GetComponent<LoopingSound>();

        ToggleVFX(false);

        startHeight = transform.localPosition.z;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrilling)
        {
            ToggleVFX(true);
            lame.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * 100f, Space.Self);
        }
        else
        {
            ToggleVFX(false);
        }
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
