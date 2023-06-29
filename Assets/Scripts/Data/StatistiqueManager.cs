using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Filliere
{
    Viticulture,
    GrandeCulture,
    Elevage,
    JEV
}

public class StatistiqueManager : MonoBehaviour
{
    public Statistique statsViti;
    public Statistique statsGC;
    public Statistique statsEle;
    public Statistique statsJEV;

    public Dictionary<Filliere, Statistique> filliereToStatsDictionary { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        string json = DataManager.ReadFromFile("Statistiques/Viticulture.json");
        statsViti = JsonConvert.DeserializeObject<Statistique>(json);

        string json2 = DataManager.ReadFromFile("Statistiques/GrandeCulture.json");
        statsGC = JsonConvert.DeserializeObject<Statistique>(json2);

        string json3 = DataManager.ReadFromFile("Statistiques/Elevage.json");
        statsEle = JsonConvert.DeserializeObject<Statistique>(json3);

        string json4 = DataManager.ReadFromFile("Statistiques/JEV.json");
        statsJEV = JsonConvert.DeserializeObject<Statistique>(json4);

        StartCoroutine(AddToDictionary());
    }

    IEnumerator AddToDictionary()
    {
        filliereToStatsDictionary = new Dictionary<Filliere, Statistique>();
        yield return new WaitForSeconds(5);
        try
        {
            filliereToStatsDictionary.Add(Filliere.Viticulture, statsViti);
            filliereToStatsDictionary.Add(Filliere.GrandeCulture, statsGC);
            filliereToStatsDictionary.Add(Filliere.Elevage, statsEle);
            filliereToStatsDictionary.Add(Filliere.JEV, statsJEV);
            Debug.Log("Dictionary initialized");
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
