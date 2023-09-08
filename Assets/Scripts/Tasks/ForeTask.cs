using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeTask : TaskPedalez
{
    [SerializeField] GameObject dirtPrefab;
    [SerializeField] float holeSize = 37f;
    [SerializeField] bool lastFore;
    ForageQuest forage;
    GameObject dirt;

    [SerializeField] GameObject showCam;

    Offset dirtOffset;

    float oldProgress;

    [SerializeField] Tariere _Tariere;

    protected override void HandleBeforePedale()
    {
        base.HandleBeforePedale();
        

        forage = (ForageQuest)quest;
        if (forage.foret)
        {
            dirtOffset = new Offset(forage.foret.dirtPosition);
        }
        else
        {
            GameManager.Instance.SwitchCam(CamTypes.LookAt, showCam, false, 0, transform);

            PlayerController p = GameManager.Instance.player;

            Vector3 playerPos =new Vector3(transform.position.x, p.transform.position.y, transform.position.z+1f);
            Quaternion playerRot = Quaternion.Euler(Vector3.up * 180f);

            p.transform.position = playerPos;
            p.transform.rotation = playerRot;

            dirtOffset = new Offset(transform);

            _Tariere.gameObject.SetActive(true);
            
            GameManager.Instance.velo.ChangePente(45);
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
        }else if (!forage.foret)
        {
            GameManager.Instance.velo.ChangePente(45);
        }

        dirt.transform.parent = transform;
    }

    protected override void OnProgessChanged(float p)
    {

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


        float endDrillPos = Mathf.Lerp(_Tariere.startHeight,_Tariere.endHeigth, p / duration);

        _Tariere.transform.localPosition = new Vector3(_Tariere.transform.localPosition.x, 
                                                        _Tariere.transform.localPosition.y, 
                                                            endDrillPos);

        float vecParam = Mathf.Lerp(0, holeSize, p / duration);

        dirt.transform.localScale = Vector3.one * vecParam;

        oldProgress = p;

    }
}
