using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuveQuest : Quest
{
    public CleaningMaterial cleaningMaterial;
    public override void Interact()
    {
        base.Interact();
        StartCoroutine(GameManager.Instance.player.SwitchControls("Character", true));
    }
}
