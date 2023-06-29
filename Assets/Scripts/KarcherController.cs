using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarcherController : MonoBehaviour
{
    [SerializeField] GameObject[] karcherObjects;
    Animator anim;
    [SerializeField] GameObject waterStream;
    [SerializeField] AudioClip karcherSFX;
    [SerializeField] AudioClip equipSFX;
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < karcherObjects.Length; i++)
        {
            karcherObjects[i].SetActive(false);
        }
        anim = GetComponent<PlayerController>().playerAnim;
        waterStream.SetActive(false);
    }


    public void GetKarcher(GameObject objToRemove = null)
    {
        if (objToRemove)
        {
            objToRemove.SetActive(false);
        }
        GameManager.Instance.player.canMove = false;
        GameManager.Instance.GetComponent<TransitionManager>().FadeTransition(1f, 1f, 2f);
        Invoke("ShowEquipments", 2f);
    }

    void ShowEquipments()
    {
        AudioManager.instance.PlaySFX(equipSFX);
        for (int i = 0; i < karcherObjects.Length; i++)
        {
            karcherObjects[i].SetActive(true);
        }
        anim.SetBool("Karcher", true);
        GameManager.Instance.player.canMove = true;
    }

    public void UseKarcher()
    {
        anim.SetTrigger("UseKarcher");
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Epaule, 4f);
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Main, 4f);
        RecapManager.instance.medicalRecap.AddInjurie(Parts.Coude, 4f);
        Invoke("EnableWaterStream", 1f);
    }

    public void RemoveKarcher()
    {
        for (int i = 0; i < karcherObjects.Length; i++)
        {
            karcherObjects[i].SetActive(false);
        }
        anim.SetBool("Karcher", false);
    }

    void EnableWaterStream()
    {
        waterStream.SetActive(true);
        AudioManager.instance.PlaySFX(karcherSFX,1,true,1.5f);
        Invoke("DisableWaterStream", 1.5f);
    }

    void DisableWaterStream()
    {
        waterStream.SetActive(false);
    }
}
