using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public DialogueVelo velo;
    public GameObject player;

    public int pente;

    void Start()
    {

    }

    private int penteA;
    // Update is called once per frame
    void Update()
    {

        if(penteA != pente) { velo.pente(pente); penteA = pente; }
        
        player.transform.position += velo.instantaneousPower * Time.deltaTime * player.transform.forward;

    }




}
