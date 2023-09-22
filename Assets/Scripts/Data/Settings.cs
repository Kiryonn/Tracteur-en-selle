using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    SerieDeTheme,
    ContreLaMontre
}

[System.Serializable]public class Difficulty
{
    public string difficultyName;
    public float maxPente; // default is 5 for max value
    public float minPente;
}

[System.Serializable]
public class LevelDesc
{
    public Theme th;
    public string sceneName;
}

[CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings")]
public class Settings : ScriptableObject
{
    public float volume;
    public float grassDensity;
    public float viewDistance;
    public List<LevelDesc> levelDescs;
    public List<Theme> allowedThemes;
    public List<Theme> currentTheme;
    public bool enableTutorial;
    public GameMode gameMode;
    public float maxTimeForTimedRun;
    public List<Difficulty> difficulties;
    public Difficulty currentDifficulty;

    public bool allowStats;
}
