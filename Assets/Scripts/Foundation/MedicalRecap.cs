using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedicalRecap : MonoBehaviour
{
    Dictionary<Parts, float> bodyDico;
    [SerializeField] List<BodyParts> bodyParts;
    public GameObject recapCanva;
    // Start is called before the first frame update
    void Start()
    {
        bodyDico = new Dictionary<Parts, float>();
        /*foreach (var item in bodyParts)
        {
            bodyDico.Add(item, 0f);
        }*/
        //recapCanva.SetActive(false);
    }

    public void AddInjurie(Parts part, float amount)
    {
        if (bodyDico.ContainsKey(part))
        {
            bodyDico[part] += amount;
        }
        else
        {
            bodyDico.Add(part, amount);
        }
        
        foreach (KeyValuePair<Parts, float> bDico in bodyDico)
        {
            MyDebug.Log("La partie : " + bDico.Key.ToString() + " est endommagé à hauteur de : " + bDico.Value);
        }

        UpdateRecap();
    }

    public void SetDictionnary(Dictionary<Parts, float> dico)
    {
        bodyDico = dico;
    }

    public void UpdateRecap()
    {
        foreach (var item in bodyParts)
        {
            if (bodyDico.ContainsKey(item.partie))
            {
                float pain = bodyDico[item.partie];
                if (pain > 10f)
                {
                    pain = 10;
                }
                item.painPoint.SetFloat("Size", pain*2);
                //item.painPoint.Play();
                //item.painPoint.rectTransform.localScale = Vector3.one * pain / 5;
                /*
                 * - Un message de statistique
                 * - 
                 */
            }
            
        }
    }

    public void ShowMedicalRecap(float duration)
    {
        foreach (var item in bodyParts)
        {
            if (bodyDico.ContainsKey(item.partie))
            {
                item.painPoint.SetFloat("Duration", duration);
                item.painPoint.Play();
            }
        }
    }

    public bool Injured()
    {
        foreach (KeyValuePair<Parts, float> bDico in bodyDico)
        {
            if (bDico.Value > 0)
            {
                MyDebug.Log("Injured worker");
                return true;
            }
        }
        MyDebug.Log("Healthy worker");
        return false;
    }

    public void ClearInjuries()
    {
        foreach (var item in bodyParts)
        {
            if (bodyDico.ContainsKey(item.partie))
            {
                bodyDico[item.partie] = 0f;
            }
            item.painPoint.Stop();
            item.painPoint.Reinit();
        }
        UpdateRecap();
    }
}
