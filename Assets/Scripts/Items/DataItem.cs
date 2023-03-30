using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataTypes
{
    Age,
    Categorie,
}

public class DataItem : Item
{

    [SerializeField] DataTypes dataTypes;
    [SerializeField] string dataField;
    TutorielQuest tutorielQuest;

    public override void Interact()
    {
        base.Interact();
        tutorielQuest = (TutorielQuest)GameManager.Instance.currentQuest;
        switch (dataTypes)
        {
            case DataTypes.Age:
                tutorielQuest.client.Age = dataField;
                break;
            case DataTypes.Categorie:
                tutorielQuest.client.Categorie = dataField;
                break;
            default:
                break;
        }
    }
}
