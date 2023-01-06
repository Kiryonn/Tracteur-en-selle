using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    public ZoneAction[] idObjet;

    private List<string> idObjetif;

    private Dictionary<string, ZoneAction> recherche;



    private Dictionary<string, List<string> > condition; // la condition c et pour les tache de la liste de t

    public Dictionary<string,bool> realiser; // nom des tache et leur status 


    private void Start()
    {
        idObjetif=new List<string>();
        condition=new Dictionary<string, List<string>>();
        recherche = new Dictionary<string, ZoneAction>();
        foreach (ZoneAction t in idObjet)
        {
            t.manageMe(this);
            idObjetif.Add(t.name);
            recherche.Add(t.name, t);

            foreach (string i in t.condition) 
            {
                try
                {
                    condition[i].Add(t.name);
                }
                catch (Exception)
                {
                    condition[i] = new List<string>();
                    condition[i].Add(t.name);
                    Debug.Log(""+i+" /"+ t.name);
                    //throw;
                }

            }
           
        }


        realiser =new Dictionary<string,bool>();
        realiser.Add("true", true);
        foreach (string t in idObjetif)
        {
            realiser.Add(t,false);
        }
        Debug.Log("recherche " + recherche["true"].name);
        Debug.Log("condition " + condition.ToString());
        Debug.Log("realiser " + realiser.ToString());

    }

    public bool isgood(string[] lsname)
    {
        
        foreach (string t in lsname)
        {
            Debug.Log("" + t + " = " + realiser[t]);

            if (!realiser[t]) { return false; }
        }
        return true;
    }

    public int nbgood(string[] lsname)
    {
        int n = 0;
        foreach (string t in lsname)
        {
            //Debug.Log("" + t + " = " + realiser[t]);

            if (!realiser[t]) { n++; }
        }
        return n;
    }

    public void isRealiser(string name) 
    {
        //Debug.Log("realiser A" + realiser[name]);
        realiser[name] = true;
        //Debug.Log("realiser Ap" + realiser[name]);

        foreach (string t in condition[name])//verif d'actualisation 
        {
            try
            {
                recherche[t].active();
            }
            catch (Exception)
            {

                //throw;
            }
            
        }
        
    }

    public void plusBesoin(string[] lsname)
    {
        foreach (string t in lsname)//verif d'actualisation 
        {
            try
            {
                recherche[t].disparition();
            }
            catch (Exception)
            {

                //throw;
            }

        }

    }



}
