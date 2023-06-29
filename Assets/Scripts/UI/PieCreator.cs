using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PieCreator : MonoBehaviour
{
    [SerializeField] GameObject uIRoot;

    [SerializeField] Sprite circleSprite;

    [Header("Test Values")]
    [SerializeField] int[] testValues;


    public void CreatePieFromComponent()
    {
        CreatePie(uIRoot, circleSprite, testValues);
    }

    public static Color[] CreatePie(GameObject uIRoot, Sprite circleSprite, int[] values)
    {
        Color[] colors = new Color[values.Length];

        Array.Sort(values);

        uIRoot.transform.Clear();

        int sum = values.Sum();

        Image img;

        float rapport = 1 / ((float)values[values.Length - 1] / (float)sum);
        Color rngColor;
        float lastFillAmount = 1f;
        // Mettons la premiere valeur en fond à 1 (totalement remplie)

        float percentage = (float)values[0] / sum;
        RectTransform temp = new GameObject("PiePart", typeof(RectTransform)).GetComponent<RectTransform>();
        temp.SetParent(uIRoot.transform, false);

        img = temp.gameObject.AddComponent<Image>();
        img.sprite = circleSprite;

        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Radial360;
        img.fillAmount = 1f;

        rngColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

        img.color = rngColor;
        SetAndStretchToParentSize(temp, uIRoot.GetComponent<RectTransform>());
        colors[0] = rngColor;
        // Pour chaque valeur on prend en compte la dernière valeur et on diminue de la
        for (int i = 1; i < values.Length; i++)
        {
            percentage = (float)values[i-1] / (float)sum;
            temp = new GameObject("PiePart", typeof(RectTransform)).GetComponent<RectTransform>(); // Creation d'un empty rect transform
            //temp.SetParent(uIRoot.transform,false); // On le positionne dans le canvas en conservant la position 0.0.0
            SetAndStretchToParentSize(temp,uIRoot.GetComponent<RectTransform>());
            img = temp.gameObject.AddComponent<Image>();
            img.sprite = circleSprite;

            img.type = Image.Type.Filled;
            img.fillMethod = Image.FillMethod.Radial360;
            img.fillAmount = lastFillAmount-percentage;

            lastFillAmount = img.fillAmount;

            rngColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            img.color = rngColor;

            colors[i] = rngColor;
        }

        return colors;
    }

    public static void SetAndStretchToParentSize(RectTransform _mRect, RectTransform _parent)
    {
        //_mRect.anchoredPosition = _parent.position;
        _mRect.anchorMin = new Vector2(0, 0);
        _mRect.anchorMax = new Vector2(1, 1);
        _mRect.pivot = new Vector2(0.5f, 0.5f);
        //_mRect.sizeDelta = _parent.rect.size;
        _mRect.transform.SetParent(_parent,false);
    }
}

public static class TransformEx
{
    public static Transform Clear(this Transform transform)
    {
        int childs = transform.childCount - 1;

        for (int i = childs; i >= 0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
        return transform;
    }
}
