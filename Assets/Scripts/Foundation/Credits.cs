using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject settingsButton;
    [SerializeField] Image creditButton;
    [SerializeField] Button nextButton;
    bool finished = false;

    [SerializeField] Animator[] firstDroneWave;
    [SerializeField] Animator[] secondDroneWave;
    int droneIndex = 0;

    [SerializeField] Transform[] cows;

    Dictionary<Transform, Vector3> transformToVectorDico = new Dictionary<Transform, Vector3>();

    [SerializeField] float cowsSpeed;
    [SerializeField] float cowsDuration;
    bool shaking = false;
    [SerializeField] GameObject screen;

    CinemachineImpulseSource impulseSource;

    bool isStarted = false;

    private void Start()
    {
        for (int i = 0; i < cows.Length; i++)
        {
            cows[i].gameObject.SetActive(false);
            transformToVectorDico.TryAdd(cows[i], cows[i].transform.position);
        }
        nextButton.gameObject.SetActive(false);
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void StartCredits()
    {
        if (isStarted) { EndCredits(); return; }

        isStarted = true;

        playButton.SetActive(false);

        creditButton.GetComponent<Button>().interactable = false;

        LeanTween.rotateLocal(screen, new Vector3(0f, 90f, 0f), 1f).
            setOnComplete(() =>
            {
                LeanTween.moveLocalY(screen, screen.transform.localPosition.y + 20f, 2f).setEaseInExpo();
            });

        quitButton.SetActive(false);
        settingsButton.SetActive(false);

        LeanTween.value(creditButton.gameObject, Color.white, Color.red, 1f).
            setOnUpdate((Color c) =>
            {
                creditButton.color = c;
            });

        StartCoroutine(DronePresentation(2f,firstDroneWave));

    }

    IEnumerator DronePresentation(float delay, Animator[] drones)
    {
        creditButton.GetComponent<Button>().interactable = false;
        nextButton.gameObject.SetActive(true);
        nextButton.interactable = false;

        yield return new WaitForSeconds(delay);
        for (int i = 0; i < drones.Length; i++)
        {
            drones[i].SetTrigger("Credit");
            yield return new WaitForSeconds(5f);
        }

        nextButton.interactable = true;
        creditButton.GetComponent<Button>().interactable = true;
    }

    public void NextDronePresent()
    {
        if (finished)
        {
            EndCredits();
            finished = false;
        }
        else
        {
            AngryCow();
            StartCoroutine(DronePresentation(2f, secondDroneWave));
            finished = true;
        }
    }

    void AngryCow()
    {
        StartCoroutine(CallTheCows(cowsDuration));
    }
    IEnumerator Rumble(float duration)
    {
        shaking = true;
        while (shaking)
        {
            impulseSource.GenerateImpulse();
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator CallTheCows(float duration)
    {
        //StartCoroutine(Rumble(duration));
        for (int i = 0; i < cows.Length; i++)
        {
            cows[i].gameObject.SetActive(true);
        }

        for (float i = 0f; i < 1f; i += Time.deltaTime / duration)
        {
            for (int j = 0; j < cows.Length; j++)
            {
                cows[j].position += Vector3.left * cowsSpeed * Random.Range(0.9f, 1.1f) * Time.deltaTime;
            }
            
            yield return null;
        }
        shaking = false;
        for (int i = 0; i < cows.Length; i++)
        {
            cows[i].position = transformToVectorDico[cows[i]];
        }
    }

    void EndCredits()
    {
        AngryCow();
        isStarted = false;
        finished = false;
        nextButton.gameObject.SetActive(false);
        playButton.SetActive(true);

        LeanTween.rotateLocal(screen, new Vector3(0f, 180f, 0f), 1f);
        LeanTween.moveLocalY(screen, screen.transform.localPosition.y - 20f, 2f).setEaseInExpo();

        quitButton.SetActive(true);
        settingsButton.SetActive(true);

        LeanTween.value(creditButton.gameObject, Color.red, Color.white, 1f).
            setOnUpdate((Color c) =>
            {
                creditButton.color = c;
            });
    }
}
