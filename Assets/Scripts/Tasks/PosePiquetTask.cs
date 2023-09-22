using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosePiquetTask : Task
{
    ForageQuest q;

    [SerializeField] GameObject piquetPrefab;

    [SerializeField] ToolsAnimation mass;

    Animator animator;
    PlayerController p;

    protected override void OnStart()
    {
        base.OnStart();
        mass.gameObject.SetActive(false);
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
        p = GameManager.Instance.player;

        Vector3 playerPos = new Vector3(transform.position.x-1f, p.transform.position.y, transform.position.z+0.3f);
        Quaternion playerRot = Quaternion.Euler(Vector3.up * 90f);

        animator = p.PlayAltAnim("Mass");
        p.transform.position = playerPos;
        p.transform.rotation = playerRot;
        p.canMove = false;
        p.characterController.enabled = false;
        StartCoroutine(CheckAnimationFinished());

        /*
        Vector3 startScale = mass.transform.localScale;
        mass.transform.localScale = Vector3.zero;
        mass.gameObject.SetActive(true);

        LeanTween.scale(mass.gameObject, startScale, 0.2f).setEase(LeanTweenType.easeInOutBounce)
        .setOnComplete(() =>
        {
            mass.TriggerAnimation();

            
        });
        */
        Piquet piquet = temp.GetComponent<Piquet>();
        Transform dirt = q.holesPositions.Dequeue();

        LeanTween.scale(dirt.gameObject, Vector3.zero, 1f).setOnComplete(() =>
        {
            dirt.gameObject.SetActive(false);
        });
        /*
        LeanTween.scale(mass.gameObject, Vector3.zero, 0.2f).setDelay(1f).setOnComplete(() =>
        {
            mass.gameObject.SetActive(false);
        });
        */
        temp.transform.position = new Vector3(transform.position.x, piquet.deepness, transform.position.z);
        if (q.lastPique)
        {
            piquet.AttachPique(q.lastPique);
        }

        q.lastPique = piquet;

        temp.transform.parent = transform;

        base.Interact();
    }

    IEnumerator CheckAnimationFinished()
    {
        
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return new WaitForEndOfFrame();
        }

        p.canMove = true;
        p.characterController.enabled = true;
        p.AlternateCharacter(true);
    }
}
