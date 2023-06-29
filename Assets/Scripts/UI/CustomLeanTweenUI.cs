using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLeanTweenUI : MonoBehaviour
{
    private void Start()
    {
        
    }
    [System.Serializable]
    public class SendMessageInfo
    {
        
    }
    [SerializeField] List<SendMessageInfo> sendMessages;
    public void OnClick()
    {
    }

    [SerializeField]
    GraphicSettings graphicSettings;
    public void OnDensity(float f)
    {
        graphicSettings.UpdateDensity(f);
    }

    public void OnDistance(float f)
    {
        graphicSettings.UpdateDistance(f * 250f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SettingsManager.instance.ForceLoadMainMenu();
    }
}
