using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public enum DropdownType
{
    Difficulty,
    GameMode,
    PlayerSelection
}

public class DropDownUpdater : MonoBehaviour
{
    TMP_Dropdown dropdown;
    [SerializeField]DropdownType dropdownType;
    [SerializeField] bool clearOptions = true;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        if (clearOptions) { dropdown.ClearOptions(); }
        
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
                MyDebug.Log(dropdown.value);
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
            case DropdownType.PlayerSelection:
                StartCoroutine(DropDownInitAsync());
                break;
            default:
                break;
        }
    }

    public void Refresh()
    {
        StartCoroutine(DropDownInitAsync());
    }
    IEnumerator DropDownInitAsync(float timeout = 10f)
    {
        float i = 0f;
        TMP_Dropdown.OptionData dropdownOption;
        dropdown.ClearOptions();

        dropdownOption = new TMP_Dropdown.OptionData();
        dropdownOption.text = "Aucun";
        dropdown.options.Add(dropdownOption);

        while (i < timeout)
        {
            i += Time.deltaTime;
            yield return new WaitForSeconds(1f);
            if (DataManager.instance.isLoaded)
            {
                foreach (var item in DataManager.instance.clientDataList)
                {
                    dropdownOption = new TMP_Dropdown.OptionData();
                    dropdownOption.text = item.ID;
                    dropdown.options.Add(dropdownOption);
                }
                yield break;
            }
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
            case DropdownType.PlayerSelection:
                if (dropdown.value == 0)
                {
                    SettingsManager.instance.scoreDataManager.playerData
                        .playerNickname = "";
                }
                else
                {
                    SettingsManager.instance.scoreDataManager.playerData
                        .playerNickname = dropdown.options[dropdown.value].text;
                }
                break;
            default:
                break;
        }
        
    }
}
