using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSecateur : Interactable
{
    public Vigne vigne;
    public bool affutage;
    [SerializeField] Vector3 locationOffset;
    [SerializeField] Vector3 rotationOffset;
    [SerializeField] float transitionDuration;
    [SerializeField] float usingDuration;

    [SerializeField] Animator affutageAnim;
    [SerializeField] Animator affilageAnim;
    Transform playerTransform;

    protected override void OnStart()
    {
        base.OnStart();
        playerTransform = GameManager.Instance.player.gameObject.transform;
        HideInteractable();
    }

    public override void Interact()
    {
        base.Interact();
        Use();
    }

    public void Use()
    {
        if (affutage)
        {
            vigne.secateur.Affutage();
            Debug.Log("Affutage du secateur " + vigne.secateur.type.ToString());
        }
        else
        {
            vigne.secateur.Affilage();
            Debug.Log("Affilage du secateur " + vigne.secateur.type.ToString());
        }
        StartCoroutine(MovePlayerToPosition(transitionDuration, affutage));
    }

    IEnumerator MovePlayerToPosition(float duration,bool affut)
    {
        GameManager.Instance.player.canMove = false;
        Quaternion endRot = Quaternion.Euler(rotationOffset);
        Vector3 startPos = playerTransform.localPosition;
        Quaternion startRot = playerTransform.localRotation;
        for (float i = 0.0f; i< 1.0f; i +=  Time.deltaTime / duration)
        {
            playerTransform.localPosition = Vector3.Lerp(startPos, locationOffset, i);
            playerTransform.localRotation = Quaternion.Lerp(startRot, endRot, i);
            yield return null;
        }
        playerTransform.localPosition = locationOffset;
        playerTransform.localRotation = Quaternion.Euler(rotationOffset);

        vigne.secaAnim.SetBool("Repairing",true);
        vigne.secaAnim.SetTrigger("Repair");
        if (affut)
        {
            affutageAnim.SetTrigger("Open");
        }
        yield return new WaitForSeconds(usingDuration);
        vigne.secaAnim.SetBool("Repairing", false);
        GameManager.Instance.player.canMove = true;
    }
}
