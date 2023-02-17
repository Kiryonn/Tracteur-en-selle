using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secateur : Item
{
    public SecateurTypes type;

    public int maxDurability;
    public int currentDurability;

    public bool affutage;

    public override void OnStart()
    {
        base.OnStart();
        currentDurability = maxDurability;
    }

    public void Use(int amount)
    {
        Debug.Log("Using secateur");
        currentDurability -= amount;
        switch (type)
        {
            case SecateurTypes.Electrique:
                break;
            case SecateurTypes.Rotatif:
                break;
            case SecateurTypes.Normal:
                break;
            default:
                break;
        }
        if (currentDurability < maxDurability - 5)
        {
            Debug.Log("Besoin d'affuter");
            affutage = true;
        }
        else
        {
            affutage = false;
        }
    }

    public override void Interact()
    {
        base.Interact();
        Vigne v = (Vigne)GameManager.Instance.currentQuest;
        v.SetSecateur(this);
    }

    public void Affilage()
    {
        if (!affutage)
        {
            currentDurability = maxDurability;
        }
    }

    public void Affutage()
    {
        if (affutage)
        {
            currentDurability = maxDurability - 5;
        }
    }
}
