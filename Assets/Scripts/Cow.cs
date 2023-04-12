using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CowBlend
{
    public float blendValue;
    public float percentage;
    public bool troll;
}

public class Cow : MonoBehaviour
{
    [SerializeField] CowBlend[] cowBlendValues;
    int selectedAnim;
    Animator anim;
    [SerializeField] Vector2 timeBetweenChanges;
    float currentTimer;
    float maxTimer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        ChangeAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer > maxTimer)
        {
            ChangeAnimation();
        }
    }

    Vector2 RandomSelect()
    {
        int index = Random.Range(0, cowBlendValues.Length);
        CowBlend selectedBlend = cowBlendValues[index];
        if (selectedBlend.troll)
        {
            float f = Random.Range(0, 100);
            if (selectedBlend.percentage > f)
            {
                return new Vector2(selectedBlend.blendValue,index);
            }
            else
            {
                return RandomSelect();
            }
        }
        else
        {
            return new Vector2(selectedBlend.blendValue,index);
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

        float currentBlend = cowBlendValues[selectedAnim].blendValue;
        Vector2 temp = RandomSelect();
        float nextBlend = temp.x;
        selectedAnim = (int)temp.y;
        StartCoroutine(LerpAnimation(2f, currentBlend, nextBlend));
    }
}
