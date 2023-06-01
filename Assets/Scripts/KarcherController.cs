using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarcherController : MonoBehaviour
{
    [SerializeField] GameObject[] karcherObjects;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < karcherObjects.Length; i++)
        {
            karcherObjects[i].SetActive(false);
        }
        anim = GetComponent<PlayerController>().playerAnim;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetKarcher()
    {
        for (int i = 0; i < karcherObjects.Length; i++)
        {
            karcherObjects[i].SetActive(true);
        }
        anim.SetBool("Karcher", true);
    }

    public void UseKarcher()
    {
        anim.SetTrigger("UseKarcher");
    }
}
