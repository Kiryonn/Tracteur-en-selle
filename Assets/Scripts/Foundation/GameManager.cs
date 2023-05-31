using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum GameState
{
	QuestState,
	ScoreState,
	Pause,
	CountDown
}

public enum CamTypes
{
	Tractor,
	Equipments,
	Character,
	Cinematic,
	Show,
	LookAt
}

[System.Serializable]
public class CameraObjects
{
	public GameObject camera;
	public CamTypes camTypes;
}

public class GameManager : MonoBehaviour
{


	public static GameManager Instance;
	GameState currentState;
	NightTime nTime;
	public Camera cam { get; private set; }

	[Header("Vélo")]
	public DialogueVelo velo;
	public PlayerController player { get; private set; }
	public Drone drone;
	//public GameObject player;
	public int pente;
	public Slider slider;
	[SerializeField] Transform spawnPosition;
	[SerializeField] Transform playerParent;
	

	[Header("Quetes objets et tâches")]
	public List<Quest> remainingQuests = new List<Quest>();
	public List<Quest> completedQuests = new List<Quest>();
	public Quest currentQuest;
	public List<Task> remainingTasks = new List<Task>();
	public List<Item> collectedItems = new List<Item>();
	[HideInInspector]
	public List<Item> allItems = new List<Item>();
    int totalQuest = 0;
	int totalFailedQuests = 0;

	[System.NonSerialized] public UnityEvent<Item> onCollectedItem = new UnityEvent<Item>();
	[System.NonSerialized] public UnityEvent<Quest,string> onCreatedQuest = new UnityEvent<Quest,string>();

	float timer = 0f;
	float maxTime = 0f;
	float hasStartedFloat = 1f;
	bool isPaused;

	public PlayerData playerData;
	public InteractionProperties interactionProperties;

	[Header("UI")]

	public Transform itemUIRoot;

	[Header("Cameras")]
	[SerializeField] List<CameraObjects> cameras;
	GameObject currentCamera;
	CamTypes currentCamType;
	void Awake()
	{
		if(Instance != null)
        {
			Destroy(this);
        }
        else
        {
			Instance = this;
        }
		
		cam = Camera.main;
	}

    private void Start()
    {
		UIManager.instance.SetQuestListener();
		nTime = GetComponent<NightTime>();
		itemUIRoot.gameObject.SetActive(false);
		player = velo.gameObject.GetComponent<PlayerController>();
		Invoke("SetPenteScaledWithDmg", 9f);
		currentState = GameState.QuestState;
		SpawnPlayer();
		
		
	}

    private void Update()
    {
        switch (currentState)
        {
            case GameState.QuestState:

				timer += Time.deltaTime;
				UIManager.instance.timerText.text = Mathf.RoundToInt(maxTime-timer).ToString();

                if (maxTime > 0f && timer>maxTime)
				{
					WinGame();
				}
                break;
            case GameState.ScoreState:
				timer = 0;
                break;
			case GameState.CountDown:
				break;
			case GameState.Pause:
				
				break;
            default:
                break;
        }
    }

	public void SwitchCam(CamTypes camType = CamTypes.Tractor, GameObject specialCamera = null, bool limited = false, float duration = 0f, Transform newTarget = null)
    {
		
        if (limited)
        {
			StartCoroutine(SwitchBackCam(currentCamType, duration));
        }
        if (specialCamera && currentCamera != specialCamera)
        {
			//specialCamera.SetActive(true);
			foreach (var item in cameras)
			{
				item.camera.SetActive(false);
			}
			currentCamera = specialCamera;
			
		}
        else
        {
			foreach (var item in cameras)
			{
				if (item.camTypes == camType)
				{
					//item.camera.SetActive(true);
					currentCamera = item.camera;
					currentCamType = camType;
				}
				else
				{
					item.camera.SetActive(false);
				}
			}
		}
		if (newTarget)
		{
            CinemachineVirtualCamera vCam = currentCamera.GetComponent<CinemachineVirtualCamera>();
			vCam.LookAt = newTarget;
        }
		currentCamera.SetActive(true);
		/*
        switch (camType)
        {
            case CamTypes.Tractor:
				CharaCm.SetActive(false);
				CineCm.SetActive(false);
				TractorCm.SetActive(true);
				break;
            case CamTypes.Character:
				TractorCm.SetActive(false);
				CineCm.SetActive(false);
				CharaCm.SetActive(true);
				break;
            case CamTypes.Cinematic:
				TractorCm.SetActive(false);
				CharaCm.SetActive(false);
				CineCm.SetActive(true);
				break;
            default:
                break;
        }
		*/
    }

	IEnumerator SwitchBackCam(CamTypes cam,float duration)
	{
		yield return new WaitForSeconds(duration);
		SwitchCam(cam);
	}

	public void SpawnPlayer(bool groundcheck = false)
    {
		if (groundcheck && currentState != GameState.QuestState) { return; }
		velo.transform.SetParent(playerParent);
		player.characterController.enabled = false;
		velo.transform.position = spawnPosition.position;
		velo.transform.rotation = spawnPosition.rotation;
		//Debug.Log("Setting up player position");
		Theme cur = SettingsManager.instance.settings.currentTheme;
		/*
        if (!player.isCharacterControlled) StartCoroutine(player.SwitchControls("Tractor", false));
        if (player.isCharacterControlled) StartCoroutine(player.SwitchControls("Character", false));
		*/
        if (cur == Theme.Tutorial || cur == Theme.Garage)
		{
			StartCoroutine(player.SwitchControls("Character", false));
        }
		else
		{
            StartCoroutine(player.SwitchControls("Tractor", false));
        }

        if (SettingsManager.instance.settings.gameMode == GameMode.ContreLaMontre && SettingsManager.instance.settings.currentTheme != Theme.Tutorial)
        {
            UIManager.instance.timerText.transform.parent.gameObject.SetActive(true);
            maxTime = SettingsManager.instance.settings.maxTimeForTimedRun;
        }
        else
        {
            UIManager.instance.timerText.transform.parent.gameObject.SetActive(false);
            maxTime = 0f;
        }
		player.canMove = true;
        
	}

    public void WinGame()
    {
		Debug.Log("You win");
		Debug.Log("Temps passé : " + timer);
		Debug.Log("Tracteur endommagé à :"+ (100f - velo.GetComponent<DamageController>().health));
		Debug.Log("Nombre d'échecs : " + totalFailedQuests);
		Debug.Log("Votre score est de : " + CalculateScore());
		playerData.score = CalculateScore();
		float durabilite = velo.gameObject.GetComponent<DamageController>().health;
		gameObject.GetComponent<TransitionManager>().SetValues(timer, totalFailedQuests, durabilite, CalculateScore());
		currentState = GameState.ScoreState;
		
		nTime.SetDayTime(true);
		nTime.dayNightCycle = false;
		totalFailedQuests = 0;
		collectedItems = new List<Item>();
		//SettingsManager.instance.LoadNextLevel();
    }

	

	public GameObject UISpawnObject(GameObject item)
    {
		itemUIRoot.gameObject.SetActive(true);
		GameObject temp = Instantiate(item, itemUIRoot);
		temp.transform.localPosition = Vector3.zero;
		return temp;
    }

	public void HideUIObject(GameObject item)
    {
		item.SetActive(false);
		itemUIRoot.gameObject.SetActive(false);
    }

	public void FailTask()
    {
		totalFailedQuests += 1;
    }

	public void CompleteQuest(Quest quest)
    {
		remainingQuests.Remove(quest);
		completedQuests.Add(quest);
		quest.HideInteractable();
		onCreatedQuest.Invoke(quest, "");
		if (remainingQuests.Count == 0)
        {
			WinGame();
        }
        else
        {
			ShowQuests();
			FocusOnNearestQuest();
		}
		
    }

	public void CollectItem(Item item)
    {
		if (!collectedItems.Contains(item))
        {
			if (!item.noDroneRequest) drone.SummonDrone(item);
			collectedItems.Add(item);
			onCollectedItem.Invoke(item);
		}
    }

	public void ShowQuests()
    {
        foreach (var item in remainingQuests)
        {
			item.ShowInteractable();
        }
    }

	public void HideAllObjectsOfType(Type type)
    {
        switch (type.ToString())
        {
			case "Item":
				//Debug.Log("hiding an item");
				foreach (var item in allItems)
				{
					item.HideInteractable();
				}
				break;
			case "Task":
				//Debug.Log("hiding a task");
				break;
			case "Quest":
				foreach (var item in remainingQuests)
				{
					item.HideInteractable();
				}
				//Debug.Log("hiding a quest");
				break;
			default:
                break;
        }
    }

	public void AddQuest(Quest q)
    {
		//Debug.Log("Addind "+q._name);
		remainingQuests.Add(q);
		onCreatedQuest.Invoke(q,q._name);
    }

	float CalculateScore()
    {
		float life = velo.GetComponent<DamageController>().health;
		return ((10000 / (timer + 10)) + (life * 10) - (totalFailedQuests * 100)) * 10 + completedQuests.Count * 1000f;
    }

	public void SwitchState(GameState newGameState)
    {
		currentState = newGameState;
    }

    public void SetPenteScaledWithDmg()
    {
        DamageController dmg = player.GetComponent<DamageController>();
		float rp = dmg.health / dmg.maxHealth;
		//Debug.Log("Health with rapport = " + dmg.health);
		//Debug.Log("Rapport = " + ((1 - rp) * 7));
		Difficulty diff = SettingsManager.instance.settings.currentDifficulty;

        velo.ChangePente(Mathf.RoundToInt(
			Mathf.Lerp(
				diff.minPente,diff.maxPente, 1 - rp
				)
			)
		);
    }

	public void PauseGame()
	{
		
		if (!isPaused)
		{
			isPaused = true;
			UIManager.instance.pauseMenu.SetActive(true);
            SwitchState(GameState.Pause);
		}
		else
		{
			isPaused = false;
            UIManager.instance.pauseMenu.SetActive(false);
            SwitchState(GameState.QuestState);
        }
	}

	public void CameraFocus(Transform lookObject, float howLong = 3f)
	{
        SwitchCam(CamTypes.LookAt, null, true, howLong,lookObject);
    }

	public void FocusOnNearestQuest()
	{
		float dist = 0f;
		float currDist;
		Quest nearestQuest = null;
        foreach (var item in remainingQuests)
        {
            currDist = Vector3.Distance(player.transform.position,item.transform.position);
			if (dist == 0f || currDist < dist)
			{
				dist = currDist;
				nearestQuest = item;
			}
        }

		if (nearestQuest == null) { return; }

		SwitchCam(CamTypes.LookAt,null,true,3f,nearestQuest.transform);
    }
}
