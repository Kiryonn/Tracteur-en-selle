using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapManager : MonoBehaviour
{
    List<Recap> recaps;
    public MedicalRecap medicalRecap { get; private set; }
    public static RecapManager instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        recaps = new List<Recap>();
        medicalRecap = GetComponent<MedicalRecap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRecap(Recap re)
    {
        recaps.Add(re);
    }
    /*
     * Exemple :
     * Vous avez pris le risque : X% du temps
     * Recap aléatoire : En moyenne X% du temps les gens font patatipatata
     * Votre bilan santé : Schéma avec des bulles rouges qui montre les zones les plus touché
     * échelle bilan de santé = 10f pour maximum
     */
}
