using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField] AudioClip[] footClip;
    int randomIndex;
    public void PlayeStepSound()
    {
        randomIndex = Random.Range(0, footClip.Length);
        AudioManager.instance.PlaySFX(footClip[randomIndex]);
    }
}
