using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBox : TutorialBubble
{
    [SerializeField] GameObject dialogueBox;
    RectTransform rectTransform;
    float startHeight;
    protected override void OnStart()
    {
        base.OnStart();
        rectTransform = dialogueBox.GetComponent<RectTransform>();
        startHeight = rectTransform.sizeDelta.y;
    }

    protected override IEnumerator FadeTutorial(float duration, float endAlpha)
    {

        if (endAlpha == 1f)
        {
            indications.SetActive(true);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);
        }
        else
        {
            rectTransform.LeanSize(new Vector2(rectTransform.sizeDelta.x, 0), 2f).setEaseInBack();
            yield return new WaitForSeconds(1f);
        }

        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1 - endAlpha, endAlpha, i);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
        rectTransform.LeanSize(new Vector2(rectTransform.sizeDelta.x, startHeight), 2f).setEase(LeanTweenType.linear);

        if (endAlpha == 0f) indications.SetActive(false);
    }
}
