using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public Settings settings;

    public static SettingsManager instance;
    int[] levelOrder;
    Dictionary<Theme, string> dicoThemes;
    bool nextIsGarage;
    Scene loadedLevel;
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
        nextIsGarage = true;
        dicoThemes = new Dictionary<Theme, string>();
        foreach (var item in settings.levelDescs)
        {
            dicoThemes.TryAdd(item.th, item.sceneName);
        }
        if (settings.allowedThemes == null)
        {
            settings.allowedThemes = new List<Theme>();
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
            if (settings.allowedThemes.Contains(t)){ return; }
            settings.allowedThemes.Add(t);
        }
        else
        {
            settings.allowedThemes.Remove(t);
        }
        
    }


    public void UpdateGrassDensity(float f)
    {
        settings.grassDensity = f;
    }

    public void UpdateViewDistance(float f)
    {
        settings.viewDistance = f;
    }
    public void StartGame()
    {
        try
        {
            LoadingScreenTips.instance.StartLoading();
        }
        catch (System.Exception)
        {
            Debug.Log("Missing Component");
            throw;
        }
        StartCoroutine(LoadSceneAsyncScreen(1));
        
        //nextIsGarage = false;
        //loadedLevel = SceneManager.GetSceneByName("Garage");
        //LoadNextLevel();
    }

    IEnumerator LoadSceneAsyncScreen(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        while (!operation.isDone)
        {
            LoadingScreenTips.instance.loadgingBarFill.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }

        AsyncOperation operation2;
        if (settings.enableTutorial)
        {
            operation2 = SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive);
            loadedLevel = SceneManager.GetSceneByName("Tutorial");
            nextIsGarage = true;
        }
        else
        {
            operation2 = SceneManager.LoadSceneAsync("Garage", LoadSceneMode.Additive);
            loadedLevel = SceneManager.GetSceneByName("Garage");
            nextIsGarage = false;
        }

        while (!operation2.isDone)
        {
            LoadingScreenTips.instance.loadgingBarFill.fillAmount = Mathf.Clamp01(operation2.progress / 0.9f);
            yield return null;
        }

    }

    public void LoadNextLevel()
    {
        SceneManager.UnloadSceneAsync(loadedLevel);

        if (settings.allowedThemes.Count <= 0)
        {
            Debug.Log("No more level");
            settings.currentTheme = Theme.None;
        }
        else
        {
            if (nextIsGarage)
            {
                SceneManager.LoadScene("Garage", LoadSceneMode.Additive);
                nextIsGarage = false;
                loadedLevel = SceneManager.GetSceneByName("Garage");
                settings.currentTheme = Theme.Garage;
            }
            else
            {
                int r = Random.Range(0, settings.allowedThemes.Count);
                SceneManager.LoadScene(dicoThemes[settings.allowedThemes[r]],LoadSceneMode.Additive);
                loadedLevel = SceneManager.GetSceneByName(dicoThemes[settings.allowedThemes[r]]);
                settings.currentTheme = settings.allowedThemes[r];
                settings.allowedThemes.Remove(settings.allowedThemes[r]);
                
                nextIsGarage = true;
            }
        }
        
    }

    public void LoadMainMenu()
    {
        if (settings.currentTheme == Theme.None)
        {
            foreach (var item in settings.levelDescs)
            {
                settings.allowedThemes.Add(item.th);
            }
            Debug.Log("Reloading all levels");
            SceneManager.LoadScene("Main Menu");
        }
    }
}
