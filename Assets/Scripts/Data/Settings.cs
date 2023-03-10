using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings")]
public class Settings : ScriptableObject
{
    public float volume;
    public float grassDensity;
    public float viewDistance;
    public List<LevelDesc> levelDescs;
    public List<Theme> allowedThemes;
    public Theme currentTheme;


    [System.Serializable]
    public class LevelDesc
    {
        public Theme th;
        public string sceneName;
    }

    
}
