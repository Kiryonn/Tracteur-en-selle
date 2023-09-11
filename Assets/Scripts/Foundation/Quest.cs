using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : Interactable
{
    [SerializeField] protected List<Task> requiredTasks;
    public float bandDelay = 1f;
    public MSAVision vision;
    public bool isStarted { get { return (this == GameManager.Instance.currentQuest); } }
    [SerializeField] bool arrowPointing;
    [SerializeField] GameObject arrowPrefab;
    GameObject arrow;

    protected bool counting;
    protected float elapsedTime = 0f;
    protected override void OnStart()
    {
        GameManager.Instance.AddQuest(this);
        if (vision) { vision.HideInteractable(); }
        GetComponent<Renderer>().material.SetColor("_Color", GameManager.Instance.interactionProperties.taskColor);
        if (arrowPointing)
        {
            arrow = Instantiate(arrowPrefab);
            arrow.transform.SetParent(transform, false);
            arrow.transform.localPosition = Vector3.forward * 3f;
        }
        
    }

    public override void ShowInteractable()
    {
        base.ShowInteractable();
        if (arrow != null) { arrow.SetActive(true); }
        
    }

    public override void HideInteractable()
    {
        base.HideInteractable();
        if (arrow != null) { arrow.SetActive(false); }
    }

    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.TriggerQuestStart(this);
        StartCoroutine(StartTimer());
        StartQuest();
    }

    protected IEnumerator StartTimer()
    {
        counting = true;
        while (counting)
        {
            elapsedTime += Time.unscaledTime;
            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual void StartQuest()
    {
        if (requiredTasks.Count <= 0)
        {
            counting = false;
            GameManager.Instance.CompleteQuest(this,elapsedTime);
            return;
        }
        GameManager.Instance.HideAllObjectsOfType(typeof(Quest));
        GameManager.Instance.remainingTasks = requiredTasks;
        GameManager.Instance.currentQuest = this;

        HideInteractable();
        NextTask();
    }

    public virtual void CompleteTask(Task task)
    {
        task.HideInteractable();
        requiredTasks.Remove(task);
        if (requiredTasks.Count > 0)
        {
            NextTask();
        }
        else
        {
            HandleCompletedQuest();
        }
    }

    protected virtual void NextTask()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.soundData.popClip);
        requiredTasks[0].ShowInteractable();
        requiredTasks[0].SetQuest(this);
        if (requiredTasks[0].requireItem)
        {
            foreach (var item in requiredTasks[0].requiredObjects)
            {
                item.SetQuest(this);
                item.ShowInteractable();
            }
            if (vision) { vision.ShowInteractable(); }
        }
        else
        {
            if (vision) { vision.HideInteractable(); }
        }
        
    }

    public Task GetCurrentTask()
    {
        return requiredTasks[0];
    }

    public bool isFinished()
    {
        return requiredTasks.Count <= 0;
    }

    protected virtual void HandleCompletedQuest()
    {
        if (vision)
        {
            vision.HideInteractable();
        }
        counting = false;
        GameManager.Instance.CompleteQuest(this,elapsedTime);
        
        GameManager.Instance.currentQuest = null;
    }
}
