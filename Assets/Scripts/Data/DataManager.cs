using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public class DataManager : MonoBehaviour
{
	public ClientData[] clientData { get; private set; }
	public List<ClientData> clientDataList;
	public static DataManager instance;
	void Start()
	{
		if (instance == null)
        {
			instance = this;
        }
        else
        {
			Destroy(this);
        }
		string json = ReadFromFile("VisiteursData.json");
		clientData = JsonConvert.DeserializeObject<ClientData[]>(json);
	}

	public static string ReadFromFile(string fileName)
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

	void SaveJson(string fileName)
	{
		string path = Application.streamingAssetsPath + "/" + fileName;
		string content = JsonConvert.SerializeObject(clientData);
		Debug.Log("Content = " + content);
		File.WriteAllText(path, content);
	}

	public void UpdateVisiteurData(ClientData data)
    {
		try
        {
			clientDataList = clientData.ToList<ClientData>();
		}
        catch (System.Exception)
        {
			clientDataList = new List<ClientData>();
            //throw;
        }
		SettingsManager.instance.scoreDataManager.AddPlayer(data.ID);
		clientDataList.Add(data);
		clientData = clientDataList.ToArray();
		SaveJson("VisiteursData.json");
    }
}