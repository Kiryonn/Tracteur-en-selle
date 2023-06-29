using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Collections;

public class ScoreDataManager : MonoBehaviour
{
    Dictionary<string, float> scores;
    DataScore[] dataFromJSON;
    string player = "";
    private void Start()
    {
        scores = new Dictionary<string, float>();
        string json = ReadFromFile("ScoreData.json");
        dataFromJSON = JsonConvert.DeserializeObject<DataScore[]>(json);

        StartCoroutine(InitScores(5f));
    }

    IEnumerator InitScores(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < dataFromJSON.Length; i++)
        {
            scores.TryAdd(dataFromJSON[i].ID, dataFromJSON[i].score);
        }
    }

    public void AddPlayer(string id)
    {
        scores.TryAdd(player, 0f);
    }

    string ReadFromFile(string fileName)
    {
        string path = Application.streamingAssetsPath + "/" + fileName;
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.Log("No such file");
        }

        return "";
    }

    public void AddScore(float score)
    {
        try
        {
            scores[player] = score;
            UpdateScoreData(new DataScore(player, scores[player]));
        }
        catch (System.Exception)
        {
            Debug.Log("Can't change player score of id : " + player);
        }
    }

    void SaveJson(string fileName, DataScore[] data)
    {
        string path = Application.streamingAssetsPath + "/" + fileName;
        string content = JsonConvert.SerializeObject(data);
        Debug.Log("Content = " + content);
        File.WriteAllText(path, content);
    }

    

    public void UpdateScoreData(DataScore data)
    {
        DataScore[] dataScores = new DataScore[scores.Count];
        SaveJson("VisiteursData.json",dataScores);
    }
}
