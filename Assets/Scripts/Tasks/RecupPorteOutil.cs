using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecupPorteOutil : EquipmentRecup
{
    [SerializeField] Vector3 playerOffset;
    [SerializeField] GameObject displayedForet;

    GameObject obj;
    public override void Interact()
    {
        GameManager.Instance.player.charaGraphics.transform.localPosition = playerOffset;

        GameManager.Instance.player.SetEquipment(this,false);

        obj = Instantiate(equipmentPrefab);
        obj.transform.parent = GameManager.Instance.player.transform;
        offset.SetOffset(obj.transform);
        ForageQuest forage = (ForageQuest)quest;
        forage.foret = obj.GetComponent<Foret>();
        forage.isEquipped = true;
        
        GameManager.Instance.player.playerAnim.SetBool("Mount", true);

        HideInteractable();
        GameManager.Instance.CollectItem(this,true);
        AudioManager.instance.PlaySFX(AudioManager.instance.soundData.recupClip);
        foreach (var item in linkedItems)
        {
            item.HideInteractable();
        }
        displayedForet.SetActive(false);

        forage.GetCurrentTask().ShowInteractable();
    }

    public override void RemoveEquipment()
    {
        GameManager.Instance.player.canMove = false;
        GameManager.Instance.player.playerAnim.SetBool("Mount", false);
        GameManager.Instance.GetComponent<TransitionManager>().FadeTransition(1f, 3f, 1f);
        Invoke("ResetPlayer", 3f);
    }

    void ResetPlayer()
    {
        displayedForet.SetActive(true);
        GameManager.Instance.player.charaGraphics.transform.localPosition = Vector3.zero;
        GameManager.Instance.player.canMove = true;
        obj.SetActive(false);
    }

}
