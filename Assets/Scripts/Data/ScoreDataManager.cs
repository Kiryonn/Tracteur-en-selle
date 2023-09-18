using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Collections;

public class ScoreDataManager : MonoBehaviour
{
    public Dictionary<string, (float,float)> scores { get; private set; }
    public PlayerData playerData;
    DataScore[] dataFromJSON;
    public Dictionary<string,QuestTime> questsStats { get; private set; }
    QuestTime[] questTimes;
    string json;
    private void Start()
    {
        scores = new Dictionary<string, (float,float)>();
        json = ReadFromFile("ScoreData.json");

        dataFromJSON = JsonConvert.DeserializeObject<DataScore[]>(json);

        playerData.playerNickname = "";
        playerData.score = 0f;

        StartCoroutine(InitScores(3f));

        questsStats = new Dictionary<string, QuestTime>();
        string json2 = ReadFromFile("Statistiques/Quetes/StatsParQuete.json");

        questTimes = JsonConvert.DeserializeObject<QuestTime[]>(json2);

        StartCoroutine(InitQuetes(2f));
    }

    IEnumerator InitScores(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < dataFromJSON.Length; i++)
        {
            scores.TryAdd(dataFromJSON[i].ID, (dataFromJSON[i].score,dataFromJSON[i].time));
        }
    }

    IEnumerator InitQuetes(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < questTimes.Length; i++)
        {
            questsStats.TryAdd(questTimes[i]._Name,questTimes[i]);
        }
    }

    public void AddQuest(string _name, float time)
    {
        QuestTime quest = new QuestTime(_name, time);
        if (questsStats.ContainsKey(_name))
        {
            quest._Temps = (questsStats[_name]._Temps + quest._Temps) / 2f;
            questsStats.TryAdd(_name, quest);
        }
        else
        {
            questsStats.TryAdd(_name, quest);
        }
        UpdateQuestData();
    }

    public void AddPlayer(string id)
    {
        playerData.playerNickname = id;
        scores.TryAdd(id, (0f,0f));
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
            MyDebug.Log("No such file");
        }

        return "";
    }

    public void AddScore(float score, float time)
    {
        MyDebug.Log("Trying to add a score of " + score + " to the player : " + playerData.playerNickname);
        playerData.time = (playerData.time + time) / 2f;
        try
        {
            if (playerData.playerNickname == "")
            {
                playerData.playerNickname = DataManager.instance.GenerateUser();
                scores[playerData.playerNickname] = (score, playerData.time);
            }
            else if(score > scores[playerData.playerNickname].Item1)
            {
                scores[playerData.playerNickname] = (score, playerData.time);
            }

            UpdateScoreData();
        }
        catch (System.Exception)
        {
            MyDebug.Log("Can't change player score of id : " + playerData.playerNickname);
        }
    }

    static void SaveJson(string fileName, DataScore[] data)
    {
        string path = Application.streamingAssetsPath + "/" + fileName;
        string content = JsonConvert.SerializeObject(data);
        MyDebug.Log("Content = " + content);
        File.WriteAllText(path, content);
    }
    static void SaveJson(string fileName, QuestTime[] data)
    {
        string path = Application.streamingAssetsPath + "/" + fileName;
        string content = JsonConvert.SerializeObject(data);
        MyDebug.Log("Content = " + content);
        File.WriteAllText(path, content);
    }

    public void UpdateScoreData()
    {
        DataScore[] dataScores = new DataScore[scores.Count];
        int i = 0;
        foreach (KeyValuePair<string, (float,float)> entry in scores)
        {
            dataScores[i] = new DataScore(entry.Key, entry.Value.Item1, entry.Value.Item2);
            i++;
        }

        SaveJson("ScoreData.json",dataScores);
    }

    public void UpdateQuestData()
    {
        QuestTime[] qTime = new QuestTime[questsStats.Count];
        int i = 0;
        foreach (KeyValuePair<string,QuestTime> item in questsStats)
        {
            qTime[i] = new QuestTime(item.Key, item.Value._Temps);
            i++;
        }
        SaveJson("Statistiques/Quetes/StatsParQuete.json", qTime);

    }

    public void SaveAndReset()
    {
        string path = Application.streamingAssetsPath + "/" + "ScoreData.json";
        string destPath = Application.streamingAssetsPath + "/Archives/";
        File.Copy(path, destPath + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + "ScoreData.json");

        path = Application.streamingAssetsPath + "/" + "VisiteursData.json";
        destPath = Application.streamingAssetsPath + "/Archives/";
        File.Copy(path, destPath + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + "VisiteursData.json");

        SaveJson("ScoreData.json", new DataScore[0]);
        scores.Clear();
        DataManager.instance.ResetFile();
    }


}
