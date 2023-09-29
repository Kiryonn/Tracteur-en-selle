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
    LookAt,
    Generator
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
    public Camera cam;
    public Camera camSecond;

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
    int totalSucceededTasks = 0;
    int totalFailedTasks = 0;

    [System.NonSerialized] public UnityEvent<Item> onCollectedItem = new UnityEvent<Item>();
    [System.NonSerialized] public UnityEvent<Quest, string> onCreatedQuest = new UnityEvent<Quest, string>();
    [System.NonSerialized] public UnityEvent<Quest,float> onStartQuest = new UnityEvent<Quest,float>();
    [System.NonSerialized] public UnityEvent<Quest> onCompleteTask = new UnityEvent<Quest>();
    [System.NonSerialized] public UnityEvent<Quest> onCompleteQuest = new UnityEvent<Quest>();

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

    [Header("SFX")]
    [SerializeField] AudioClip failSFX;
    [SerializeField] AudioClip successSFX;

    [Header("Raptors")]

    [SerializeField] RaptorSpawner raptorSpawner;
    void Awake()
    {
        if (Instance != null)
        {
            MyDebug.Log("GAMEMANAGER IS DESTROYED");
            Destroy(this);
        }
        else
        {
            Instance = this;
            MyDebug.Log("GAMEMANAGER IS INSTANCE");
        }

        if (!cam)
            cam = Camera.main;
    }

    private void Start()
    {
        UIManager.instance.SetQuestListener();
        nTime = GetComponent<NightTime>();
        itemUIRoot.gameObject.SetActive(false);
        player = velo.gameObject.GetComponent<PlayerController>();

        StartCoroutine(InitPente(9f));
        currentState = GameState.QuestState;
        SpawnPlayer();

        currentState = GameState.Pause;

        UIManager.instance.allOfUI.SetActive(false);
    }

    public void SpawnTheRaptors()
    {
        raptorSpawner.SpawnRaptors();
    }

    public void StartGame()
    {
        UIManager.instance.allOfUI.SetActive(true);
        currentState = GameState.QuestState;
        //SpawnTheRaptors();
    }
    IEnumerator InitPente(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetPenteScaledWithDmg();
    }
    private void Update()
    {
        switch (currentState)
        {
            case GameState.QuestState:

                timer += Time.deltaTime;
                UIManager.instance.timerText.text = Mathf.RoundToInt(maxTime - timer).ToString();

                if (maxTime > 0f && timer > maxTime)
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
                    if (currentCamera)
                        currentCamera.SetActive(false);

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

    IEnumerator SwitchBackCam(CamTypes cam, float duration)
    {
        MyDebug.Log("Switching cam for : " + duration);
        MyDebug.Log("The camera that we want to go back to is : " + cam.ToString());
        yield return new WaitForSeconds(duration);
        MyDebug.Log("Switching camera back to : " + cam.ToString());
        SwitchCam(cam,null,false);
    }

    public void SpawnPlayer(bool groundcheck = false)
    {
        if (groundcheck && currentState != GameState.QuestState) { return; }
        if (groundcheck)
        {
            player.GetComponent<DamageController>().DamageTractor(20f);
        }
        velo.transform.SetParent(playerParent);
        player.characterController.enabled = false;
        velo.transform.position = spawnPosition.position;
        velo.transform.rotation = spawnPosition.rotation;
        //MyDebug.Log("Setting up player position");

        
        /*
        if (!player.isCharacterControlled) StartCoroutine(player.SwitchControls("Tractor", false));
        if (player.isCharacterControlled) StartCoroutine(player.SwitchControls("Character", false));
		*/
        if (SettingsManager.instance.isGarageOrTutorial())
        {
            StartCoroutine(player.SwitchControls("Character", false));
        }
        else
        {
            StartCoroutine(player.SwitchControls("Tractor", false));
        }

        if (SettingsManager.instance.settings.gameMode == GameMode.ContreLaMontre && SettingsManager.instance.settings.currentTheme[0] != Theme.Tutorial)
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
        MyDebug.Log("You win");
        MyDebug.Log("Temps passé : " + timer);
        MyDebug.Log("Tracteur endommagé à :" + (100f - velo.GetComponent<DamageController>().health));
        MyDebug.Log("Nombre d'échecs : " + totalFailedTasks);
        playerData.score = CalculateScore();

        MyDebug.Log("Votre score est de : " + playerData.score);
        
        float durabilite = velo.gameObject.GetComponent<DamageController>().health;
        gameObject.GetComponent<TransitionManager>().SetValues(timer, totalFailedTasks, durabilite, playerData.score);
        SettingsManager.instance.scoreDataManager.AddScore(playerData.score, timer);

        currentState = GameState.ScoreState;
        

        nTime.SetDayTime(true);
        nTime.dayNightCycle = false;
        totalFailedTasks = 0;
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
        totalFailedTasks += 1;
        MyDebug.Log("Task is failed, current failure is equal to : " + totalFailedTasks);
        AudioManager.instance.PlaySFX(AudioManager.instance.soundData.failClip,1,true);
        onCompleteTask.Invoke(currentQuest);
    }

    public void CompleteQuest(Quest quest, float time)
    {
        remainingQuests.Remove(quest);
        completedQuests.Add(quest);
        quest.HideInteractable();
        onCreatedQuest.Invoke(quest, "");
        onCompleteQuest.Invoke(quest);

        SettingsManager.instance.scoreDataManager.AddQuest(quest._name, time);

        MyDebug.Log("COMPLETING QUEST OF TYPE : " + quest._name);
        if (remainingQuests.Count == 0)
        {
            Invoke("WinGame", 2f);
            //WinGame();
        }
        else
        {
            ShowQuests();
            MyDebug.Log("I am invoking focus on quest");
            Invoke("FocusOnNearestQuest", 4f);
        }

    }

    public void CollectItem(Item item, bool noAnim = false)
    {
        if (!collectedItems.Contains(item))
        {
            if (!item.noDroneRequest) drone.SummonDrone(item);
            collectedItems.Add(item);
            onCollectedItem.Invoke(item);
            if (player.isCharacterControlled && !noAnim)
            {
                player.playerAnim.SetTrigger("Take");
            }
        }
    }

    public void ShowQuests()
    {
        foreach (var item in remainingQuests)
        {
            item.ShowInteractable();
        }
    }

    public void TriggerQuestStart(Quest q)
    {
        onStartQuest.Invoke(q,q.bandDelay);
    }

    public void HideAllObjectsOfType(Type type)
    {
        switch (type.ToString())
        {
            case "Item":
                //MyDebug.Log("hiding an item");
                foreach (var item in allItems)
                {
                    item.HideInteractable();
                }
                break;
            case "Task":
                //MyDebug.Log("hiding a task");
                break;
            case "Quest":
                foreach (var item in remainingQuests)
                {
                    item.HideInteractable();
                }
                //MyDebug.Log("hiding a quest");
                break;
            default:
                break;
        }
    }

    public void AddQuest(Quest q)
    {
        //MyDebug.Log("Addind "+q._name);
        remainingQuests.Add(q);
        q.ShowInteractable();
        onCreatedQuest.Invoke(q, q._name);
    }

    float CalculateScore()
    {
        float life = velo.GetComponent<DamageController>().health * 10;
        MyDebug.Log("Score debug 1 : " + (1 - (timer / SettingsManager.instance.settings.maxTimeForTimedRun) + 0.5f));
        float timeCal = (1 - (timer / SettingsManager.instance.settings.maxTimeForTimedRun) /*+ 0.5f*/) * 30000;
        float failCal = 1f/(totalFailedTasks+1f);
        float questCal = completedQuests.Count * 1000f;
        float succCal = totalSucceededTasks * 87.5f;

        MyDebug.Log("Here is the detailed score : " +
            timeCal+" + "+life+" + "+questCal+" + "+succCal+" * "+failCal);
        return ( timeCal + life + questCal + succCal)* failCal;
    }

    public void SwitchState(GameState newGameState)
    {
        currentState = newGameState;
    }

    public void SetPenteScaledWithDmg(float value = -1f)
    {

        DamageController dmg = player.GetComponent<DamageController>();
        float rp = dmg.health / dmg.maxHealth;
        //MyDebug.Log("Health with rapport = " + dmg.health);
        //MyDebug.Log("Rapport = " + ((1 - rp) * 7));
        Difficulty diff = SettingsManager.instance.settings.currentDifficulty;
        if (value > -1)
        {
            velo.ChangePente(Mathf.RoundToInt(
            Mathf.Lerp(
                diff.minPente, diff.maxPente, value
                    )
                )
            );
        }
        else
        {
            velo.ChangePente(Mathf.RoundToInt(
            Mathf.Lerp(
                diff.minPente, diff.maxPente, 1 - rp
                    )
                )
            );
        }
        
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
        SwitchCam(CamTypes.LookAt, null, true, howLong, lookObject);
    }

    public void FocusOnNearestQuest()
    {
        MyDebug.Log("Focus is called");
        float dist = 0f;
        float currDist;
        Quest nearestQuest = null;
        foreach (var item in remainingQuests)
        {
            currDist = Vector3.Distance(player.transform.position, item.transform.position);
            if (dist == 0f || currDist < dist)
            {
                dist = currDist;
                nearestQuest = item;
            }
        }

        if (nearestQuest == null) { return; } 

        CameraFocus(nearestQuest.transform, 3f);
    }
    //SwitchCam(CamTypes.LookAt, null, true, 3f, nearestQuest.transform);
    public void SucceedTask(Task t)
    {
        totalSucceededTasks++;
        AudioManager.instance.PlaySFX(AudioManager.instance.soundData.sucessClip, 1,true);
        onCompleteTask.Invoke(currentQuest);
        MyDebug.Log("Task is succeeded, current sucess is equal to : " + totalSucceededTasks);
    }
}
