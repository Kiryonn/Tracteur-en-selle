using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

public class ZoneAction : MonoBehaviour
{
    public string name;
    public  string[] condition;//nom des zonne ou l'ont doit passée avant

    public MeshRenderer meshRenderer;
    private bool actif;


    public string[] objetAide;
    private bool fini=true;
    public float temps =5f;

    private bool fin=true;

    InteractionManager manager;

    private void Start()
    {
        if (condition.Length == 0) 
        {
            condition = new string[1];
            condition[0]=("true"); }

        if (condition[0] != "true") { meshRenderer.enabled = false; }
        actif = true;

        
    }


    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("colision");
        // Debug.Log(other.tag);
        if (fin)
        {
            if (other.tag == "Player")
            {
                //Debug.Log("Player");
                if (manager.isgood(condition))
                {
                    
                        Debug.Log("condition Valide");

                        action();
                    
                }
            }
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        fini = false;
        StopAllCoroutines();
        
    }

    private IEnumerator Wait() 
    { 
        yield return new WaitForSeconds(temps);
        if(fini)
        fin_action();
        Debug.Log("fin timer");
    }


   void action()
    {
        
        fini = true;
        StartCoroutine(Wait());


        
    }

    void fin_action() 
    {
        //fini = false;
        int n = manager.nbgood(objetAide);
        int chanceDeReussite = 100 - 40 * n;

        if (Random.Range(0, 100) > chanceDeReussite) { Debug.Log("acident"); }//acident a appéle 
        disparition();
        manager.isRealiser(name);
        if (n > 0)
            manager.plusBesoin(objetAide);
        
    }

    public void manageMe(InteractionManager m) 
    {
        manager = m;
    }

    public void active() 
    {
        if (actif) { 
        if (manager.isgood(condition))
        {
            meshRenderer.enabled = true;
            actif = false;
            }
            
        }

     }

    public void disparition()
    {

        fin = false;
        meshRenderer.enabled = false;
        

    }



}
