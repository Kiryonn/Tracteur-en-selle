using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public enum DropdownType
{
    Difficulty,
    GameMode
}

public class DropDownUpdater : MonoBehaviour
{
    TMP_Dropdown dropdown;
    [SerializeField]DropdownType dropdownType;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        TMP_Dropdown.OptionData dropdownOption;

        switch (dropdownType)
        {
            case DropdownType.Difficulty:
                foreach (var item in SettingsManager.instance.settings.difficulties)
                {
                    dropdownOption = new TMP_Dropdown.OptionData();
                    dropdownOption.text = item.difficultyName;
                    dropdown.options.Add(dropdownOption);
                }


                dropdown.value = SettingsManager.instance.settings.difficulties.IndexOf(SettingsManager.instance.settings.currentDifficulty);
                Debug.Log(dropdown.value);
                break;
            case DropdownType.GameMode:
                List<GameMode> gameModes = Enum.GetValues(typeof(GameMode)).Cast<GameMode>().ToList();
                foreach (var item in gameModes)
                {
                    dropdownOption = new TMP_Dropdown.OptionData();
                    dropdownOption.text = item.ToString();
                    dropdown.options.Add(dropdownOption);
                }
                dropdown.value = 1;
                break;
            default:
                break;
        }

        
    }

    public void OnDropDownChange()
    {
        switch (dropdownType)
        {
            case DropdownType.Difficulty:
                SettingsManager.instance.settings.currentDifficulty = SettingsManager.instance.settings.difficulties[dropdown.value];
                break;
            case DropdownType.GameMode:
                GameMode[] tableau = (GameMode[])Enum.GetValues(typeof(GameMode));
                SettingsManager.instance.settings.gameMode = tableau[dropdown.value];
                break;
            default:
                break;
        }
        
    }
}
