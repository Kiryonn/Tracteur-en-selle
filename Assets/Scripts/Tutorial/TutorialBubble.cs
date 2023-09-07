using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerActions
{
    Move,
    Turn,
    Wait,
    Input,
    StartQuest,
    Pedale
}
public class TutorialBubble : MonoBehaviour
{
    public bool triggered { get; private set; }
    [SerializeField] float waitingTime;
    [SerializeField] float delay;
    [SerializeField] float fadeSpeed;
    [SerializeField] bool allowMovement;
    [SerializeField] bool activated;
    bool clustered = true;
    [SerializeField] protected GameObject indications;
    [SerializeField] PlayerActions playerAction;
    protected CanvasGroup canvasGroup;
    [SerializeField] List<Interactable> requiredInteractions = new List<Interactable>();
    [SerializeField] List<TutorialBubble> requiredTuto = new List<TutorialBubble>();

    [System.NonSerialized] public UnityEvent<TutorialBubble> OnCompletedTutorialBubble = new UnityEvent<TutorialBubble>();
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = indications.GetComponent<CanvasGroup>();
        if (GetComponent<TutorialBubbleCluster>() == null)
        {
            clustered = false;
        }
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

        OnStart();
    }

    protected virtual void OnStart()
    {
        // Do anything
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (!triggered && activated && !clustered)
        {
            
            StartCoroutine("LaunchTutorial");
            
        }
    }

    IEnumerator WaitForActions(PlayerActions action)
    {
        float currentTime = 0f;
        switch (action)
        {
            case PlayerActions.Move:
                while (currentTime < waitingTime)
                {
                    if (GameManager.Instance.player.movement != 0)
                    {
                        currentTime += Time.deltaTime;
                    }
                    yield return null;
                }
                break;
            case PlayerActions.Pedale:
                int timeout = 0;
                while (GameManager.Instance.velo.speed <= 1)
                {
                    timeout++;
                    if (timeout > 5000)
                    {
                        break;
                    }
                    yield return null;
                }
                break;
            case PlayerActions.Turn:

                while (currentTime < waitingTime)
                {
                    if (GameManager.Instance.player.rotation != 0)
                    {
                        currentTime += Time.deltaTime;
                    }
                    yield return null;
                }
                break;
            case PlayerActions.Wait:
                yield return new WaitForSeconds(waitingTime + fadeSpeed);
                break;
            case PlayerActions.Input:
                while (!Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null;
                }
                break;
            default:
                break;
        }
        triggered = true;
        if (!clustered) { GameManager.Instance.player.canMove = true; }
        
        StartCoroutine(FadeTutorial(2f, 0f));
    }

    protected virtual IEnumerator FadeTutorial(float duration, float endAlpha)
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

    public virtual IEnumerator LaunchTutorial()
    {
        if (!allowMovement) GameManager.Instance.player.canMove = false;
        else GameManager.Instance.player.canMove = true;

        //StartCoroutine(WaitForActions(playerAction));
        OnCompletedTutorialBubble.Invoke(this);

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
        //Debug.Log(requiredInteractions.Count <= 0 && requiredTuto.Count <= 0);
        return (requiredInteractions.Count <= 0 && requiredTuto.Count <= 0);
    }
}
