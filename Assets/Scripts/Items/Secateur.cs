using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secateur : Item
{
    public SecateurTypes type;
    [SerializeField] float bodyDamage = 0;
    public int lameIndex { get; private set; }
    [SerializeField] int maxDurability;
    public int currentDurability;
    Vigne v;
    public bool affilage;

    protected override void OnStart()
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
                RecapManager.instance.medicalRecap.AddInjurie(Parts.Epaule, bodyDamage);
                break;
            case SecateurTypes.Rotatif:
                RecapManager.instance.medicalRecap.AddInjurie(Parts.Main, bodyDamage);
                break;
            case SecateurTypes.Normal:
                RecapManager.instance.medicalRecap.AddInjurie(Parts.Main, bodyDamage);
                break;
            default:
                break;
        }
        switch (currentDurability)
        {
            case 10:
                lameIndex = 0;
                break;
            case 8:
                lameIndex = 1;
                break;
            case 6:
                lameIndex = 2;
                break;
            case 4:
                lameIndex = 3;
                GameManager.Instance.velo.ChangePente(3);
                break;
            case 2:
                lameIndex = 4;
                GameManager.Instance.velo.ChangePente(7);
                break;
            case 0:
                lameIndex = 5;
                break;
            default:
                break;
        }
        v.UpdateSecateurSprite(lameIndex);

        if (currentDurability < maxDurability - 5)
        {
            Debug.Log("Besoin d'affuter");
            affilage = false;
            GameManager.Instance.GetComponent<TransitionManager>().FadeDamage(0.4f);
        }
        else
        {
            affilage = true;
        }
    }

    public override void Interact()
    {
        base.Interact();
        v = (Vigne)GameManager.Instance.currentQuest;
        v.SetSecateur(this);
    }

    public void Affilage()
    {
        if (affilage)
        {
            currentDurability = maxDurability;
            lameIndex = 0;
            v.UpdateSecateurSprite(0);
            GameManager.Instance.velo.ChangePente(0);
        }
    }

    public void Affutage()
    {
        currentDurability = 2;
        affilage = true;
        lameIndex = 4;
        v.UpdateSecateurSprite(4);
    }

    protected override void ItemDeliveredTrigger(Item item)
    {
        base.ItemDeliveredTrigger(item);
        if (item == this)
        {
            GameManager.Instance.player.tractorAnim.SetTrigger("OpenTrap");
        }
        
    }
}
