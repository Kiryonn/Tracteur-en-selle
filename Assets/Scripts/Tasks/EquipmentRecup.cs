using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Offset
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public Offset (Vector3 pos, Vector3 rot, Vector3 scl)
    {
        position = pos;
        rotation = rot;
        scale = scl;
    }

    public void SetOffset(Transform _object)
    {
        _object.localPosition = position;
        _object.localRotation = Quaternion.Euler(rotation);
        _object.localScale = scale;
    }
}

public class EquipmentRecup : Item
{
    [SerializeField] GameObject equipmentPrefab;
    [SerializeField] Offset offset;
    [SerializeField] bool playerParent;

    public GameObject equipment { get; private set; }


    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.player.SetEquipment(this);
        equipment = Instantiate(equipmentPrefab);

        if (playerParent)
        {
            equipment.transform.SetParent(GameManager.Instance.player.transform);
        }
        equipment.transform.localPosition = offset.position;
        equipment.transform.localRotation = Quaternion.Euler(offset.rotation);
        equipment.transform.localScale = offset.scale;
    }

    public void RemoveEquipment()
    {
        equipment.transform.parent = null;
        //ShowInteractable();
        Destroy(equipment);
    }
}
