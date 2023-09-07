using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForageQuest : Quest
{
    public Foret foret;
    [SerializeField] GameObject tractorObject;
    public Queue<Transform> holesPositions;
    [HideInInspector] public Piquet lastPique;

    protected override void OnStart()
    {
        base.OnStart();
        HideTractor();
        holesPositions = new Queue<Transform>();
    }

    public override void Interact()
    {
        base.Interact();
        StartCoroutine(GameManager.Instance.player.SwitchControls("Character", true));
        Invoke("ShowTractor", 3f);
    }

    protected override void HandleCompletedQuest()
    {
        base.HandleCompletedQuest();
        StartCoroutine(GameManager.Instance.player.SwitchControls("Tractor", true));
        Invoke("HideTractor", 3f);
    }

    void ShowTractor()
    {
        tractorObject.SetActive(true);
    }

    void HideTractor()
    {
        tractorObject.SetActive(false);
    }
}
