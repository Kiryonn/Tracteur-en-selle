using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InputFieldTimer : MonoBehaviour
{
    TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        OnValueChange();
    }

    public void OnValueChange()
    {
        float.TryParse(inputField.text, out var maxTime);
        SettingsManager.instance.settings.maxTimeForTimedRun = maxTime;
    }
}
