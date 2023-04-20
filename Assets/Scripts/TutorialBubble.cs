using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerActions
{
    Move,
    Turn,
    Wait
}
public class TutorialBubble : MonoBehaviour
{
    bool triggered;
    [SerializeField] float waitingTime;
    [SerializeField] float delay;
    [SerializeField] float fadeSpeed;
    [SerializeField] bool allowMovement;
    [SerializeField] bool activated;
    [SerializeField] GameObject indications;
    [SerializeField] PlayerActions playerAction;
    CanvasGroup canvasGroup;
    [SerializeField] List<Interactable> requiredInteractions = new List<Interactable>();
    [SerializeField] List<TutorialBubble> requiredTuto = new List<TutorialBubble>();

    [System.NonSerialized] public UnityEvent<TutorialBubble> OnCompletedTutorialBubble = new UnityEvent<TutorialBubble>();
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = indications.GetComponent<CanvasGroup>();
        if (requiredInteractions.Count <= 0 && requiredTuto.Count <= 0)
        {
            activated = true;
        }
        else
        {
            foreach (Interactable item in requiredInteractions)
            {
                item.OnInteract.AddListener(UpdateRequiredInteracts);
            }

            foreach (TutorialBubble item in requiredTuto)
            {
                item.OnCompletedTutorialBubble.AddListener(UpdateRequiredTutorial);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (!triggered && activated)
        {
            if (!allowMovement) GameManager.Instance.player.canMove = false;
            StartCoroutine("LaunchTutorial");
            StartCoroutine(WaitForActions(playerAction));
            OnCompletedTutorialBubble.Invoke(this);
        }
    }

    IEnumerator WaitForActions(PlayerActions action)
    {
        
        switch (action)
        {
            case PlayerActions.Move:
                while (GameManager.Instance.velo.speed <= 1)
                {
                    yield return null;
                }
                break;
            case PlayerActions.Turn:
                break;
            case PlayerActions.Wait:
                yield return new WaitForSeconds(waitingTime + fadeSpeed);
                break;
            default:
                break;
        }
        triggered = true;
        GameManager.Instance.player.canMove = true;
        StartCoroutine(FadeTutorial(2f, 0f));
    }

    IEnumerator FadeTutorial(float duration, float endAlpha)
    {
        if (endAlpha == 1f) indications.SetActive(true);
        
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1-endAlpha,endAlpha,i);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;

        if (endAlpha == 0f) indications.SetActive(false);

    }

    IEnumerator LaunchTutorial()
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeTutorial(fadeSpeed, 1f));
        StartCoroutine(WaitForActions(playerAction));
    }

    void UpdateRequiredInteracts(Interactable inte)
    {
        requiredInteractions.Remove(inte);
        activated = CheckActivation();
    }

    void UpdateRequiredTutorial(TutorialBubble tuto)
    {
        requiredTuto.Remove(tuto);
        activated = CheckActivation();
    }

    bool CheckActivation()
    {
        Debug.Log(requiredInteractions.Count <= 0 && requiredTuto.Count <= 0);
        return (requiredInteractions.Count <= 0 && requiredTuto.Count <= 0);
    }
}
