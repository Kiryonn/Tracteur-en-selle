using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLeanTweenUI : MonoBehaviour
{
    [System.Serializable]
    public class SendMessageInfo
    {
        
    }
    [SerializeField] List<SendMessageInfo> sendMessages;
    public void OnClick()
    {
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
