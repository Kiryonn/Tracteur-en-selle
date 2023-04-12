using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActions
{
    Move,
    Turn,
    Wait
}
public class TutorialBubble : MonoBehaviour
{
    bool triggered;
    [SerializeField] GameObject indications;
    [SerializeField] PlayerActions playerAction;
    CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = indications.GetComponent<CanvasGroup>();
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (!triggered)
        {
            GameManager.Instance.player.canMove = false;
            StartCoroutine(FadeTutorial(3f, 1f));
            StartCoroutine(WaitForActions(playerAction));
            
        }
    }

    IEnumerator WaitForActions(PlayerActions action)
    {
        yield return new WaitForSeconds(3f);
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
                yield return new WaitForSeconds(3f);
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
        
        for (float i = 0.0f; i < 1.0f; i += duration / Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1-endAlpha,endAlpha,i);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;

        if (endAlpha == 0f) indications.SetActive(false);
    }
}
