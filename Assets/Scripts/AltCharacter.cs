using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltCharacter : MonoBehaviour
{
    [SerializeField] GameObject[] mass;
    [SerializeField] GameObject[] tariere;
    [SerializeField] AudioClip bamClip;

    [SerializeField] LoopingSound loopingSound;
   
    public void HideMass()
    {
        for (int i = 0; i < mass.Length; i++)
        {
            mass[i].SetActive(false);
        }

        for (int i = 0; i < tariere.Length; i++)
        {
            tariere[i].SetActive(true);
        }
    }

    public void Hidetariere()
    {
        for (int i = 0; i < mass.Length; i++)
        {
            mass[i].SetActive(true);
        }

        for (int i = 0; i < tariere.Length; i++)
        {
            tariere[i].SetActive(false);
        }
    }

    public void PlayBamSound()
    {
        AudioManager.instance.PlaySFX(bamClip);
    }

    public void PlayDrill()
    {
        loopingSound.PlayFromStart(0, true);
    }

    public void StopDrill()
    {
        loopingSound.Stop();
    }
}
