using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiseEnBouteilleQuest : QuestOnFoot
{
    public Transpalette currentTranspalette;

    public override void Interact()
    {
        Invoke("AlignPlayer", 3.5f);
        base.Interact();
        
    }

    void AlignPlayer()
    {
        GameManager.Instance.player.transform.LookAt(requiredTasks[0].transform);
    }
}
