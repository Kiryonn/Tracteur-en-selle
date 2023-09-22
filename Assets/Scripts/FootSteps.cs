using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField] AudioClip[] footClip;
    int randomIndex;
    AudioSource source;
    private void Start()
    {
        TryGetComponent<AudioSource>(out source);


    }

    public void PlayeStepSound()
    {
        randomIndex = Random.Range(0, footClip.Length);
        AudioManager.instance.PlaySFX(footClip[randomIndex]);
    }

    public void PlayStepSoundFromSource()
    {
        randomIndex = Random.Range(0, footClip.Length);
        if (source != null)
        {
            source.PlayOneShot(footClip[randomIndex]);
        }
    }
}
