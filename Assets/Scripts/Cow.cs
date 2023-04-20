using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CowBlend
{
    public int animIndex;
    public float percentage;
    public bool troll;
}

public class Cow : MonoBehaviour
{


    [SerializeField] CowBlend[] cowBlendValues;
    [SerializeField] bool randomizeAnim;
    [SerializeField] int selectedAnim;
    Animator anim;
    [SerializeField] Vector2 timeBetweenChanges;
    float currentTimer;
    float maxTimer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if (randomizeAnim)
        {
            ChangeAnimation();
        }
        else
        {
            anim.SetInteger("AnimIndex", selectedAnim);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (randomizeAnim)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > maxTimer)
            {
                ChangeAnimation();
            }
        }
        
    }

    int RandomSelect()
    {
        int index = Random.Range(0, cowBlendValues.Length);
        CowBlend selectedBlend = cowBlendValues[index];
        if (selectedBlend.troll)
        {
            float f = Random.Range(0, 100);
            if (selectedBlend.percentage > f)
            {
                return selectedBlend.animIndex;
            }
            else
            {
                return RandomSelect();
            }
        }
        else
        {
            return selectedBlend.animIndex;
        }
    }

    IEnumerator LerpAnimation(float duration, float startBlend, float endBlend)
    {
        for (float i = 0f; i < 1.0f; i += Time.deltaTime / duration)
        {
            anim.SetFloat("Blend", Mathf.Lerp(startBlend, endBlend, i));
            yield return null;
        }
        anim.SetFloat("Blend", endBlend);
    }

    void ChangeAnimation()
    {
        currentTimer = 0f;
        maxTimer = Random.Range(timeBetweenChanges.x, timeBetweenChanges.y);
        anim.SetTrigger("ChangeAnim");
        anim.SetInteger("AnimIndex", RandomSelect());
    }
}
