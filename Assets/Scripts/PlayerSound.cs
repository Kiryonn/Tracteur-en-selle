using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    PlayerController playerController;

    float currentSpeed;
    [SerializeField] float maxSpeed;

    [SerializeField] float minPitch;
    float currentMinPitch;
    [SerializeField] float maxNormalPitch;
    [SerializeField] float maxPitch;



    public float test;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.isCharacterControlled)
        {
            audioSource.volume = 1.0f;
            EngineSound();
        }
        else
        {
            audioSource.volume = 0f;
        }
        
    }

    void EngineSound()
    {
        currentSpeed = rb.velocity.magnitude;
        currentMinPitch = Mathf.Lerp(minPitch, maxNormalPitch, Mathf.Abs(playerController.movement));
        audioSource.pitch = Mathf.Lerp(currentMinPitch, maxPitch, currentSpeed / maxSpeed * Mathf.Abs(playerController.movement));
    }
}
