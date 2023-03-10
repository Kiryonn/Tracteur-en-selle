using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSettingsButton : MonoBehaviour
{
    public Theme theme;
    Toggle toggle;
    private void Start()
    {
        toggle = GetComponent<Toggle>();
        if (toggle)
            toggle.isOn = SettingsManager.instance.settings.allowedThemes.Contains(theme);
    }

    public void OnClick(bool checkBox)
    {
        SettingsManager.instance.UpdateTheme(theme, checkBox);
    }

    public void StartGame()
    {
        SettingsManager.instance.StartGame();
    }
}
