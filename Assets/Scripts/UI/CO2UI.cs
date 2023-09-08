using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CO2UI : MonoBehaviour
{
    public Image bulb;
    [SerializeField] Image firstCircle;
    [SerializeField] Image secondCircle;
    public bool alarming;
    public bool activated;
    TransitionManager transitionManager;


    private void Start()
    {
        bulb.color = Color.green;
        StartCoroutine(Alarm());
        transitionManager = GameManager.Instance.GetComponent<TransitionManager>();
    }

    public IEnumerator Alarm()
    {
        bulb.color = Color.green;
        while (activated)
        {
            if (alarming)
            {
                bulb.color = Color.red;
                secondCircle.color = Color.black;

                firstCircle.color = Color.red;
                transitionManager.FadeDamage(0.7f);

                yield return new WaitForSeconds(1f);

                firstCircle.color = Color.black;

                secondCircle.color = Color.red;

                yield return new WaitForSeconds(1f);
            }
            else
            {
                firstCircle.color = Color.black;
                secondCircle.color = Color.black;
                bulb.color = Color.green;
            }
            
            yield return null;
        }
    }
}
