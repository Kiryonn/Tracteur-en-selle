using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatistiqueManager : MonoBehaviour
{
    public Statistique statsViti;
    public Statistique statsGC;
    public Statistique statsEle;
    public Statistique statsJEV;

    public Dictionary<Theme, Statistique> filliereToStatsDictionary { get; private set; }
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
        filliereToStatsDictionary = new Dictionary<Theme, Statistique>();
        yield return new WaitForSeconds(5);
        try
        {
            filliereToStatsDictionary.Add(Theme.Viticulture, statsViti);
            filliereToStatsDictionary.Add(Theme.Grande_Culture, statsGC);
            filliereToStatsDictionary.Add(Theme.Elevage, statsEle);
            filliereToStatsDictionary.Add(Theme.JEV, statsJEV);
            MyDebug.Log("Dictionary initialized");
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
