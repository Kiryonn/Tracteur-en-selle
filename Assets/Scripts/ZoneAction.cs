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

    //bool aciv;

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


    void action()
    {
        disparition();
        manager.isRealiser(name);
        

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


            meshRenderer.enabled = false;
        

    }



}
