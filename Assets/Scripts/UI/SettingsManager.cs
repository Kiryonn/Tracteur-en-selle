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
        SceneManager.LoadScene(1);
        SceneManager.LoadScene("Garage", LoadSceneMode.Additive);
        nextIsGarage = false;
        loadedLevel = SceneManager.GetSceneByName("Garage");
        //LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        SceneManager.UnloadSceneAsync(loadedLevel);

        if (settings.allowedThemes.Count <= 0)
        {
            foreach (var item in settings.levelDescs)
            {
                settings.allowedThemes.Add(item.th);
            }
            Debug.Log("Reloading all levels");
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            if (nextIsGarage)
            {
                SceneManager.LoadScene("Garage", LoadSceneMode.Additive);
                nextIsGarage = false;
                loadedLevel = SceneManager.GetSceneByName("Garage");
            }
            else
            {
                int r = Random.Range(0, settings.allowedThemes.Count);
                SceneManager.LoadScene(dicoThemes[settings.allowedThemes[r]],LoadSceneMode.Additive);
                loadedLevel = SceneManager.GetSceneByName(dicoThemes[settings.allowedThemes[r]]);
                settings.allowedThemes.Remove(settings.allowedThemes[r]);
                
                nextIsGarage = true;
            }
        }
        
    }


}
