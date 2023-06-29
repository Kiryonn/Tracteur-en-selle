using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSettingsButton : MonoBehaviour
{
    public Theme theme;
    public bool isTut;
    Toggle toggle;
    private void Start()
    {
        toggle = GetComponent<Toggle>();
        if (toggle)
        {
            toggle.isOn = SettingsManager.instance.settings.allowedThemes.Contains(theme);
            if (isTut) toggle.isOn = SettingsManager.instance.settings.enableTutorial;
        } 
    }

    public void OnClick(bool checkBox)
    {
        SettingsManager.instance.UpdateTheme(theme, checkBox);
    }

    public void OnClickTut(bool checkBox)
    {
        SettingsManager.instance.UpdateTutorial(checkBox);
    }

    public void StartGame()
    {
        SettingsManager.instance.StartGame();
    }
}
