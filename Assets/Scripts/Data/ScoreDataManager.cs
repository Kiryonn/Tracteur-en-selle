using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Collections;

public class ScoreDataManager : MonoBehaviour
{
    public Dictionary<string, float> scores { get; private set; }
    public PlayerData playerData;
    DataScore[] dataFromJSON;
    private void Start()
    {
        scores = new Dictionary<string, float>();
        string json = ReadFromFile("ScoreData.json");
        dataFromJSON = JsonConvert.DeserializeObject<DataScore[]>(json);
        playerData.playerNickname = "";
        playerData.score = 0f;
        StartCoroutine(InitScores(3f));
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
        playerData.playerNickname = id;
        scores.TryAdd(id, 0f);
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
        Debug.Log("Trying to add a score of " + score + " to the player : " + playerData.playerNickname);

        try
        {
            if (playerData.playerNickname == "")
            {
                playerData.playerNickname = DataManager.instance.GenerateUser();
                scores[playerData.playerNickname] = score;
            }
            else if(score > scores[playerData.playerNickname])
            {
                scores[playerData.playerNickname] = score;
            }

            UpdateScoreData(new DataScore(playerData.playerNickname, scores[playerData.playerNickname]));
        }
        catch (System.Exception)
        {
            Debug.Log("Can't change player score of id : " + playerData.playerNickname);
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
        int i = 0;
        foreach (KeyValuePair<string, float> entry in scores)
        {
            dataScores[i] = new DataScore(entry.Key, entry.Value);
            i++;
        }

        SaveJson("ScoreData.json",dataScores);
    }
}
