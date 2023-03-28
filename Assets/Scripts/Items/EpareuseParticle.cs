using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpareuseParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] string _Tag;
    int touchedHaie;

    private void Start()
    {
        particles.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag(_Tag))
        {
            touchedHaie++;
            particles.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_Tag))
        {
            touchedHaie--;
        }

        if (touchedHaie <= 0) particles.Stop();

    }
}
