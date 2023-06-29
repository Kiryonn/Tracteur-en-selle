using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMachine : MonoBehaviour
{
    public static GlobalMachine instance;
    public float globalSpeedMultiplier = 1f;
    [SerializeField] Generator generator;
    [SerializeField] float tapisSpeedMultiplier = 1f;
    [SerializeField] float workingSpeedMultiplier = 1f;
    [SerializeField] float spawnRate;
    float timer;
    [Header("Tapis")]
    [SerializeField] TakeBottle[] tapis;

    [Header("Grab")]
    [SerializeField] GrabbingArm grabbingArm;

    [Header("Filling machines")]
    [SerializeField] FillingMachine[] fillingMachines;

    [Header("Lights and effects")]
    [SerializeField] Transform[] lights;
    [SerializeField] Renderer lampeRenderer;
    Color emissionColor;

    AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        timer += spawnRate * Time.deltaTime;
        generator.multiplier = globalSpeedMultiplier; 
        if (timer > 10f)
        {
            timer = 0f;
            tapis[0].SpawBottle();
        }
        for (int i = 0; i < tapis.Length; i++)
        {
            tapis[i].multiplier = globalSpeedMultiplier * tapisSpeedMultiplier;
        }
        grabbingArm.multiplier = globalSpeedMultiplier * workingSpeedMultiplier;

        for (int i = 0;i < fillingMachines.Length; i++)
        {
            fillingMachines[i].multiplier = globalSpeedMultiplier * workingSpeedMultiplier;
        }

        audioSource.volume = Mathf.Clamp01(generator.multiplier);
    }

    public void StopAllMachines()
    {
        globalSpeedMultiplier = 0f;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.SetActive(false);
        }
        emissionColor = lampeRenderer.material.GetColor("_EmissionColor");
        lampeRenderer.material.SetColor("_EmissionColor", Color.black);
    }

    public void StartAllMachines()
    {
        globalSpeedMultiplier = 1f;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].gameObject.SetActive(true);
        }
        lampeRenderer.material.SetColor("_EmissionColor", emissionColor);
    }

    public void Spawn()
    {
        tapis[0].SpawBottle();
    }
}
