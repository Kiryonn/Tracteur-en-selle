using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBuildABetail : Quest
{
    [SerializeField] int maxGoodAnswer;
    [SerializeField] int goodAnswer;
    [SerializeField] GameObject agressiveCowPrefab;
    [SerializeField] GameObject passiveCowPrefab;
    [SerializeField] Transform agressiveSpawn;
    [SerializeField] Transform passiveSpawn;
    [SerializeField] int aggroCowAmount;

    public GameObject lastInstancedObject { get; private set; }

    protected override void OnStart()
    {
        base.OnStart();
        maxGoodAnswer = requiredTasks.Count;
    }

    public override void CompleteTask(Task task)
    {
        base.CompleteTask(task);
        AcceptPosTask acceptPosTask = (AcceptPosTask)task;

        if (acceptPosTask)
        {
            if (task.succeeded)
            {
                goodAnswer++;
            }
            lastInstancedObject = (acceptPosTask.currentOption.objInst) ? acceptPosTask.currentOption.objInst : lastInstancedObject;
        }
    }

    protected override void HandleCompletedQuest()
    {
        base.HandleCompletedQuest();
        if (goodAnswer < maxGoodAnswer)
        {
            GameObject temp;
            for (int i = 0; i<aggroCowAmount; i++)
            {
                temp = Instantiate(agressiveCowPrefab, agressiveSpawn);
                temp.transform.localPosition = Vector3.zero;
                temp.transform.localRotation = Quaternion.identity;
            }
            /*
            GameManager.Instance.player.GetComponent<DamageController>().DamageTractor(100f);
            GameManager.Instance.GetComponent<TransitionManager>().FadeDamage(1f);
            */
        }
        else
        {

        }
    }

}
