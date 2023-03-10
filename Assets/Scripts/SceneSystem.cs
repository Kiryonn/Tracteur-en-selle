using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public static SceneSystem instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadNewScene(string index)
    {
        SceneManager.LoadScene(index);
    }
}
