using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EquipmentRecup : Item
{
    [SerializeField] protected GameObject equipmentPrefab;
    [SerializeField] protected Offset offset;
    [SerializeField] protected bool playerParent;

    public GameObject equipment { get; protected set; }


    public override void Interact()
    {
        GameManager.Instance.player.SetEquipment(this);
        equipment = Instantiate(equipmentPrefab);

        if (playerParent)
        {
            equipment.transform.SetParent(GameManager.Instance.player.transform);
        }
        equipment.transform.localPosition = offset.position;
        equipment.transform.localRotation = Quaternion.Euler(offset.rotation);
        equipment.transform.localScale = offset.scale;
        base.Interact();
        
    }

    public void RemoveEquipment()
    {
        //equipment.transform.parent = null;
        //float height = transform.position.y;
        //Vector3 playerPosition = GameManager.Instance.player.transform.position;
        //transform.position = new Vector3(playerPosition.x+20f, height, playerPosition.z);
        ShowInteractable();
        Destroy(equipment);
    }

    public virtual void Use_1()
    {
        Debug.Log("First use");
    }
    public virtual void Use_2()
    {
        Debug.Log("Second use");
    }
    public virtual void Use_3()
    {
        Debug.Log("Third use");
    }
}
