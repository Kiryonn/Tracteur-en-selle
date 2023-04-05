using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecupEpareuse : Task
{
    [SerializeField] GameObject epareuse;
    Animator instanciatedEpareuse;
    [SerializeField] Vector3 pos;
    [SerializeField] Vector3 rot;
    [SerializeField] Vector3 scl;

    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.player.canMove = false;
        GameManager.Instance.GetComponent<TransitionManager>().FadeTransition(1f,3f, 1f);
        Invoke("AttachEpareuse", 3f);
    }

    void AttachEpareuse()
    {
        GameObject epa = Instantiate(epareuse, GameManager.Instance.player.transform);
        epa.transform.localPosition = pos;
        epa.transform.localRotation = Quaternion.Euler(rot);
        epa.transform.localScale = scl;
        instanciatedEpareuse = epa.GetComponent<Animator>();
        GameManager.Instance.player.canMove = true;
    }

    public void DetachEpareuse()
    {
        instanciatedEpareuse.SetTrigger("Close");
        GameManager.Instance.GetComponent<TransitionManager>().FadeTransition(1,1.5f, 0.5f);
        Destroy(instanciatedEpareuse.gameObject, 1.5f);
    }
}
