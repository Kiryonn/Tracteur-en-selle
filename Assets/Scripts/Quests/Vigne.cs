using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vigne : Quest
{
    public Secateur secateur;
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
    }

    public override void CompleteTask(Task task)
    {
        
        base.CompleteTask(task);
        if (requiredTasks.Count <= 0)
        {
            foreach (var item in machines)
            {
                item.HideInteractable();
            }
            GameManager.Instance.HideUIObject(secateurImage.gameObject);
        }
    }

    public void UpdateSecateurSprite(int index)
    {
        secateurImage.sprite = lameSprite[index];
    }
}
