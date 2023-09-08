using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public Settings settings;
    Settings savedSettings;
    public static SettingsManager instance;
    int[] levelOrder;
    Dictionary<Theme, string> dicoThemes;
    bool nextIsGarage;
    [SerializeField] List<Scene> loadedLevel;
    public ScoreDataManager scoreDataManager { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        //nextIsGarage = true;
        dicoThemes = new Dictionary<Theme, string>();
        scoreDataManager = GetComponent<ScoreDataManager>();
        foreach (var item in settings.levelDescs)
        {
            dicoThemes.TryAdd(item.th, item.sceneName);
        }
        if (settings.allowedThemes == null)
        {
            settings.allowedThemes = new List<Theme>();
        }
        //settings.gameMode = GameMode.SerieDeTheme;
        loadedLevel = new List<Scene>();

        if (settings.currentTheme == null)
        {
            settings.currentTheme = new List<Theme>();
        }
       
    }

    public void UpdateTutorial(bool b)
    {
        settings.enableTutorial = b;
    }

    public void UpdateTheme(Theme t, bool b)
    {
        if (b)
        {
            if (settings.allowedThemes.Contains(t)) { return; }
            settings.allowedThemes.Add(t);
        }
        else
        {
            settings.allowedThemes.Remove(t);
        }
    }

    void SaveSettings()
    {
        savedSettings = settings;
    }
    void LoadSettings()
    {
        settings = savedSettings;
    }
    public void StartGame()
    {
        SaveSettings();
        settings.currentTheme.Clear();
        switch (settings.gameMode)
        {
            case GameMode.SerieDeTheme:
                try
                {
                    LoadingScreenTips.instance.StartLoading();
                }
                catch (System.Exception)
                {
                    MyDebug.Log("Missing Component");
                    //throw;
                }
                if (settings.enableTutorial)
                {
                    settings.currentTheme.Add(Theme.Tutorial);
                    //nextIsGarage = true;
                    StartCoroutine(LoadSceneAsyncScreen(1, new string[] { "Tutorial" }));
                }
                else
                {
                    settings.currentTheme.Add(Theme.Garage);
                    nextIsGarage = false;
                    StartCoroutine(LoadSceneAsyncScreen(1, new string[] { "Garage" }));
                }

                break;
            case GameMode.ContreLaMontre:
                try
                {
                    LoadingScreenTips.instance.StartLoading();
                }
                catch (System.Exception)
                {
                    MyDebug.Log("Missing Component");
                    //throw;
                }
                if (settings.enableTutorial)
                {
                    settings.currentTheme.Add(Theme.Tutorial);
                    //nextIsGarage = true;
                    StartCoroutine(LoadSceneAsyncScreen(1, new string[] { "Tutorial" }));
                }
                else
                {
                    Theme[] t = settings.allowedThemes.ToArray();
                    string[] s = new string[t.Length];
                    for (int i = 0; i < s.Length; i++)
                    {
                        s[i] = t[i].ToString();
                        settings.currentTheme.Add(t[i]);
                    }
                    
                    StartCoroutine(LoadSceneAsyncScreen(1, s));
                    settings.allowedThemes.Clear();
                }
                break;
            default:
                break;
        }


        //nextIsGarage = false;
        //loadedLevel = SceneManager.GetSceneByName("Garage");
        //LoadNextLevel();
    }

    IEnumerator LoadSceneAsyncScreen(int sceneId, string[] scenesToLoad = null)
    {
        if (scenesToLoad == null) { scenesToLoad = new string[] { "Garage" }; }

        AsyncOperation operation = SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            LoadingScreenTips.instance.loadgingBarFill.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
        Camera.main.GetComponent<AudioListener>().enabled = false;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        yield return new WaitForSeconds(1f);
        AsyncOperation operation2;
        for (int i = 0; i < scenesToLoad.Length; i++)
        {
            operation2 = SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive);
            loadedLevel.Add(SceneManager.GetSceneByName(scenesToLoad[i]));
            while (!operation2.isDone)
            {
                try
                {
                    LoadingScreenTips.instance.loadgingBarFill.fillAmount = Mathf.Clamp01(operation2.progress / 0.9f);
                }
                catch { }

                yield return null;
            }
            MyDebug.Log("Trying to load " + scenesToLoad[i]);
            
            yield return null;
        }
        try
        {
            SceneManager.UnloadSceneAsync(0);
            GameManager.Instance.StartGame();
        }catch
        {
            MyDebug.Log("Could not unload first scene");
        }
        
        
        /*
        
        if (settings.enableTutorial)
        {
            operation2 = SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive);
            loadedLevel = SceneManager.GetSceneByName("Tutorial");
            settings.currentTheme = Theme.Tutorial;
            nextIsGarage = true;
        }
        else
        {
            operation2 = SceneManager.LoadSceneAsync("Garage", LoadSceneMode.Additive);
            loadedLevel = SceneManager.GetSceneByName("Garage");
            settings.currentTheme = Theme.Garage;
            nextIsGarage = false;
        }

        while (!operation2.isDone)
        {
            LoadingScreenTips.instance.loadgingBarFill.fillAmount = Mathf.Clamp01(operation2.progress / 0.9f);
            yield return null;
        }
        */
    }

    void LoadMultiple()
    {
        Theme[] t = settings.allowedThemes.ToArray();
        settings.currentTheme.Clear();
        //string[] s = new string[t.Length];
        for (int i = 0; i < t.Length; i++)
        {
            SceneManager.LoadScene(t[i].ToString(), LoadSceneMode.Additive);
            loadedLevel.Add(SceneManager.GetSceneByName(t[i].ToString()));
            settings.allowedThemes.Remove(t[i]);
            settings.currentTheme.Add(t[i]);
        }

    }

    public void LoadNextLevel()
    {
        // We first unload all of the levels that we previously loaded
        foreach (Scene scene in loadedLevel)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        loadedLevel.Clear(); // Tell our setting manager that we have no loaded scenes

        // If we were in the tutorial and in timed run, we might need to load multiple scenes
        if (settings.currentTheme[0] == Theme.Tutorial && settings.gameMode == GameMode.ContreLaMontre)
        {
            LoadMultiple();
            return;
        }
        
        

        if (settings.allowedThemes.Count <= 0)
        {
            MyDebug.Log("No more level");
            settings.currentTheme.Clear();
        }
        else
        {
            if (nextIsGarage)
            {
                SceneManager.LoadScene("Garage", LoadSceneMode.Additive);
                nextIsGarage = false;
                loadedLevel.Add(SceneManager.GetSceneByName("Garage"));
                settings.currentTheme[0] = Theme.Garage;
            }
            else
            {
                int r = Random.Range(0, settings.allowedThemes.Count);
                SceneManager.LoadScene(dicoThemes[settings.allowedThemes[r]], LoadSceneMode.Additive);
                loadedLevel.Add(SceneManager.GetSceneByName(dicoThemes[settings.allowedThemes[r]]));
                settings.currentTheme[0] = settings.allowedThemes[r];
                settings.allowedThemes.Remove(settings.allowedThemes[r]);

                //nextIsGarage = true;
            }
        }

    }

    public void LoadMainMenu()
    {
        
        if (settings.currentTheme.Count == 0)
        {
            ArduinoConnector.Instance.Close();
            foreach (var item in settings.levelDescs)
            {
                settings.allowedThemes.Add(item.th);
            }
            MyDebug.Log("Reloading all levels");
            SceneManager.LoadScene("Main Menu");
        }
        LoadSettings();
    }

    public void ForceLoadMainMenu()
    {
        ArduinoConnector.Instance.Close();
        settings.allowedThemes.Clear();
        foreach (var item in settings.levelDescs)
        {
            settings.allowedThemes.Add(item.th);
        }
        MyDebug.Log("Reloading all levels");
        SceneManager.LoadScene("Main Menu");
        LoadSettings();
    }

    public bool isGarageOrTutorial()
    {
        try
        {
            return settings.currentTheme[0] == Theme.Garage || settings.currentTheme[0] == Theme.Tutorial;
        }
        catch
        {
            return false;
        }
        
    }
}
