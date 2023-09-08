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

	public bool isLoaded { get; private set; }
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

		StartCoroutine(InitList());
	}

	IEnumerator InitList(float timeOut = 10f, float step = 1f)
    {
		isLoaded = false;
		for (float i = 0f; i < timeOut; i += Time.deltaTime)
        {
			yield return new WaitForSeconds(step);
			try
			{
				clientDataList = clientData.ToList<ClientData>();
				isLoaded = true;
				yield break;
			}
			catch (System.Exception)
			{
				// Do nothing
			}
		}
		isLoaded = true;
		clientDataList = new List<ClientData>();
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
			MyDebug.Log("No such file");
		}

		return "";
	}

	void SaveJson(string fileName)
	{
		string path = Application.streamingAssetsPath + "/" + fileName;
		string content = JsonConvert.SerializeObject(clientData);
		MyDebug.Log("Content = " + content);
		File.WriteAllText(path, content);
	}

	public string GenerateUser()
    {
		ClientData client = new ClientData();

		client.ID = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
		client.Age = "Inconnu";
		client.Categorie = "Inconnu";

		UpdateVisiteurData(client);

		return client.ID;
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