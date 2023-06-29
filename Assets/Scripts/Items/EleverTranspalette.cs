using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleverTranspalette : Item
{
    public override void Interact()
    {
        base.Interact();
        MiseEnBouteilleQuest m = (MiseEnBouteilleQuest)GameManager.Instance.currentQuest;
        m.currentTranspalette.Use();
    }
}
