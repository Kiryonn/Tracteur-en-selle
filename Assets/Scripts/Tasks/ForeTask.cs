using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForeTask : TaskPedalez
{
    [SerializeField] GameObject dirtPrefab;
    [SerializeField] float holeSize = 37f;
    [SerializeField] bool lastFore;
    ForageQuest forage;
    GameObject dirt;
    Animator altCharAnim;
    PlayerController player;
    AnimationClip tarClip;

    [SerializeField] GameObject showCam;

    Offset dirtOffset;

    float oldProgress;

    [SerializeField] Tariere _Tariere;

    [Header("UI")]

    [SerializeField] GameObject fillImageRoot;
    [SerializeField] Image fillImage;
    [SerializeField] TextMeshProUGUI percentText;

    protected override void OnStart()
    {
        base.OnStart();
        LeanTween.scale(fillImageRoot, Vector3.zero, 1f).setEaseInBounce();
    }

    public override void ShowInteractable()
    {
        if (!forage)
        {
            forage = (ForageQuest)quest;
        }
        if (forage.isEquipped)
            base.ShowInteractable();
    }

    protected override void HandleBeforePedale()
    {
        base.HandleBeforePedale();
        fillImage.fillAmount = 0f;
        percentText.text = 0 + "%";
        LeanTween.scale(fillImageRoot, Vector3.one, 1f).setEaseInBounce();

        forage = (ForageQuest)quest;
        if (forage.foret)
        {
            dirtOffset = new Offset(forage.foret.dirtPosition);
        }
        else
        {
            GameManager.Instance.SwitchCam(CamTypes.LookAt, showCam, false, 0, transform);

            player = GameManager.Instance.player;
            altCharAnim = player.PlayAltAnim("Creuse");
            altCharAnim.GetComponent<AltCharacter>().PlayDrill();

            //tarClip = altCharAnim.GetCurrentAnimatorClipInfo(0)[0].clip;

            Vector3 playerPos =new Vector3(transform.position.x, player.transform.position.y, transform.position.z+0.8f);
            Quaternion playerRot = Quaternion.Euler(Vector3.up * 180f);

            player.transform.position = playerPos;
            player.transform.rotation = playerRot;

            dirtOffset = new Offset(transform);

            //_Tariere.gameObject.SetActive(true);

            GameManager.Instance.SetPenteScaledWithDmg(0.75f);
        }
        

        dirt = Instantiate(dirtPrefab);

        dirtOffset.SetOffset(dirt.transform, Space.World);

        ForageQuest q = (ForageQuest)quest;

        q.holesPositions.Enqueue(dirt.transform);

        MyDebug.Log("Tried to enqueue an item, there is " + q.holesPositions.Count + " amount of item inside queue");
    }

    void RemoveTariere()
    {
        _Tariere.gameObject.SetActive(false);
    }

    protected override void HandleFinishPedale()
    {
        base.HandleFinishPedale();

        LeanTween.scale(fillImageRoot, Vector3.zero, 1f).setEaseInBounce();
        fillImage.fillAmount = 1f;
        percentText.text = 100 + "%";

        altCharAnim.GetComponent<AltCharacter>().StopDrill();
        player.AlternateCharacter(true);

        if (forage.foret)
        {
            forage.foret.isForage = false;
        }
        else
        {
            GameManager.Instance.SwitchCam(CamTypes.Character);
            _Tariere.isDrilling = false;
            LeanTween.scale(_Tariere.gameObject, Vector3.zero, 1f).setEaseInOutBounce();
            Invoke("RemoveTariere", 5f);

            RecapManager.instance.medicalRecap.AddInjurie(Parts.Epaule, 3f);
            RecapManager.instance.medicalRecap.AddInjurie(Parts.Dos, 2f);
            RecapManager.instance.medicalRecap.AddInjurie(Parts.Main, 1f);

        }

        if (lastFore)
        {
            Plots pl;
            GameManager.Instance.player.SetEquipment(null, false);
            GameManager.Instance.SetPenteScaledWithDmg();
            foreach (var item in necessaryItem)
            {
                if (pl = item.gameObject.GetComponent<Plots>())
                {
                    pl.RemovePlots();
                }
            }
            GameManager.Instance.player.transform.LookAt(Vector3.left);
        }
        else if (!forage.foret)
        {
            GameManager.Instance.SetPenteScaledWithDmg(0.75f);
            GameManager.Instance.player.transform.LookAt(Vector3.right);
        }

        dirt.transform.parent = transform;
    }

    protected override void OnProgessChanged(float p)
    {

        fillImage.fillAmount = p / duration;
        percentText.text = Mathf.RoundToInt(fillImage.fillAmount * 100) + "%";

        if (oldProgress != p)
        {
            if (forage.foret)
            {
                forage.foret.isForage = true;
            }
            else
            {
                _Tariere.isDrilling = true;
            }
            
        }
        else
        {
            if (forage.foret)
            {
                forage.foret.isForage = false;
            }
            else
            {
                _Tariere.isDrilling = false;
            }
        }
        altCharAnim.Play("Creuse", 0, p / duration);
        //altCharAnim.GetCurrentAnimatorClipInfo(0)[0].clip.
          //  SampleAnimation(altCharAnim.gameObject, p/duration * altCharAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
       // MyDebug.Log(altCharAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        float endDrillPos = Mathf.Lerp(_Tariere.startHeight,_Tariere.endHeigth, p / duration);

        _Tariere.transform.localPosition = new Vector3(_Tariere.transform.localPosition.x, 
                                                        _Tariere.transform.localPosition.y, 
                                                            endDrillPos);

        float vecParam = Mathf.Lerp(0, holeSize, p / duration);

        dirt.transform.localScale = Vector3.one * vecParam;

        oldProgress = p;

    }
}
