using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vigne : Quest
{
    public Secateur secateur;
    public Animator secaAnim { get; private set; }
    public List<MachineSecateur> machines;
    [SerializeField] Sprite[] lameSprite;
    Image secateurImage;
    [SerializeField] GameObject prefabSecateur;

    public void SetSecateur(Secateur s)
    {
        secateur = s;
        secateurImage = GameManager.Instance.UISpawnObject(prefabSecateur).GetComponent<Image>();
        requiredTasks[0].ShowInteractable();
        foreach (var item in machines)
        {
            item.ShowInteractable();
            item.vigne = this;
        }
        secaAnim = GameManager.Instance.velo.GetComponent<PlayerController>().secateurAnimator;
    }

    public override void CompleteTask(Task task)
    {
        
        base.CompleteTask(task);
        secaAnim.SetTrigger("Cut"); // Déclenche l'animation de coupure de vigne
        if (requiredTasks.Count <= 0)
        {
            foreach (var item in machines)
            {
                item.HideInteractable();
            }
            GameManager.Instance.HideUIObject(secateurImage.gameObject);
            secaAnim.SetTrigger("Close");
            GameManager.Instance.SetPenteScaledWithDmg();
        }
    }

    public void UpdateSecateurSprite(int index)
    {
        secateurImage.sprite = lameSprite[index];
    }

    protected override void ItemDeliveredTrigger(Item item)
    {
        if (GameManager.Instance.currentQuest == this)
        {
            secaAnim.SetTrigger("Open");
        }
        base.ItemDeliveredTrigger(item);
        
    }
}
