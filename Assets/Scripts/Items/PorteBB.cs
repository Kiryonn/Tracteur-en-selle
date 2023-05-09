using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PorteBBType
{
    Crochet,
    FourcheRonde,
    Fourche
}

public class PorteBB : EquipmentRecup
{
    [Header("Bag related")]
    public PorteBBType porteBBType;
    public PorteBBReferences bigBagSupport { get; private set; }
    public GameObject bagPrefab;
    public Offset bagOffset;

    float baseElevation;
    float baseRotation;


    [Header("Animation")]
    [SerializeField] float elevation;
    [SerializeField] float rotation;

    [Header("Player offset for align with semoir")]

    [SerializeField] Transform offsetTransform;
    [SerializeField] Offset offsetSemoir;

    public bool used { get; private set; }
    protected override void OnStart()
    {
        base.OnStart();
    }

    public override void Interact()
    {
        base.Interact();

        bigBagSupport = equipment.GetComponent<PorteBBReferences>();
    }

    public override void Use_1()
    {
        base.Use_1();
        bigBagSupport.transform.SetParent(GameManager.Instance.player.tractorGraphics.transform);
        switch (porteBBType)
        {
            case PorteBBType.Crochet:
                GameManager.Instance.player.StopAllMovements(true);

                LeanTween.rotateX(bigBagSupport.rotationCrochetPivot.gameObject, rotation, 3f).setOnComplete(() =>
                {
                    StartCoroutine(MovePlayerToPosition(3f));
                });
                break;
            case PorteBBType.FourcheRonde:
                GameManager.Instance.player.StopAllMovements(true);
                LeanTween.moveLocalY(bigBagSupport.elevationClassPivot.gameObject,
                    bigBagSupport.elevationClassPivot.localPosition.y + elevation, 3f).setOnComplete(() =>
                    {
                          StartCoroutine(MovePlayerToPosition(3f));
                    });
                break;
            case PorteBBType.Fourche:
                break;
            default:
                break;
        }
        

        //bigBagSupport.rotationCrochetPivot.Rotate(new Vector3(0, 0, rotation));
    }

    public override void Use_2()
    {
        base.Use_2();
        switch (porteBBType)
        {
            case PorteBBType.Crochet:
                LeanTween.moveLocalX(GameManager.Instance.player.gameObject,
                    GameManager.Instance.player.transform.localPosition.x + 3f, 2f).setOnComplete(() =>
                    {
                        LeanTween.rotateX(bigBagSupport.rotationCrochetPivot.gameObject, -55f, 3f);
                        GameManager.Instance.player.canMove = true;
                    });
                break;
            case PorteBBType.FourcheRonde:
                break;
            case PorteBBType.Fourche:
                break;
            default:
                break;
        }
        
    }

    IEnumerator MovePlayerToPosition(float duration)
    {
        if (offsetTransform)
        {
            offsetSemoir = new Offset(offsetTransform);
        }
        
        Transform playerTransform = GameManager.Instance.player.transform;

        Vector3 startPos = playerTransform.position;
        Quaternion startRot = playerTransform.rotation;
        Quaternion endRot = Quaternion.Euler(offsetSemoir.rotation);

        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / duration)
        {
            playerTransform.position = Vector3.Lerp(startPos, offsetSemoir.position, i);
            playerTransform.rotation = Quaternion.Lerp(startRot, endRot, i);
            yield return null;
        }

        playerTransform.position = offsetSemoir.position;
        playerTransform.rotation = endRot;
        StartCoroutine(GameManager.Instance.player.SwitchControls("Character", true, true));
        //Invoke("Use_2", 3f);
    }
}
