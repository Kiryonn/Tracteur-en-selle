using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateFillWithVelo : MonoBehaviour
{
    [SerializeField] Image textFillImage;
    DialogueVelo dialogueVelo;
    [SerializeField] float currentFill = 0;
    [SerializeField] float fillSpeed = 2f;
    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogueVelo = GetComponent<DialogueVelo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFill < dialogueVelo.cadence)
        {
            currentFill += Time.deltaTime * fillSpeed;
        }
        else
        {
            currentFill -= Time.deltaTime * fillSpeed;
        }

       
        textFillImage.fillAmount = currentFill;

        if (textFillImage.fillAmount >= 0.98f && !started)
        {
            started = true;
            Destroy(dialogueVelo.velo);
            SettingsManager.instance.StartGame();
        }
    }
}
