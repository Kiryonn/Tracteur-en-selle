using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosePiquetTask : Task
{
    ForageQuest q;

    [SerializeField] GameObject piquetPrefab;

    protected override void OnStart()
    {
        base.OnStart();
    }

    public override void ShowInteractable()
    {
        q = (ForageQuest)GameManager.Instance.currentQuest;

        base.ShowInteractable();

        Transform newPos = q.holesPositions.Peek();

        transform.position = new Vector3(newPos.position.x, transform.position.y, newPos.position.z);
    }

    public override void Interact()
    {
        GameObject temp = Instantiate(piquetPrefab);

        Piquet piquet = temp.GetComponent<Piquet>();
        Transform dirt = q.holesPositions.Dequeue();

        LeanTween.scale(dirt.gameObject, Vector3.zero, 1f).setOnComplete(() =>
        {
            dirt.gameObject.SetActive(false);
        });

        temp.transform.position = new Vector3(transform.position.x, piquet.deepness, transform.position.z);
        if (q.lastPique)
        {
            piquet.AttachPique(q.lastPique);
        }

        q.lastPique = piquet;
        
        temp.transform.parent = transform;

        base.Interact();
    }
}
