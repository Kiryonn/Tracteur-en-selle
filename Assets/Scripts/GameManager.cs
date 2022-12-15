using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public DialogueVelo velo;
	public GameObject player;
	public int pente;
	public Slider slider;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

        player.transform.position += velo.speed *Time.deltaTime * player.transform.forward;

	}

	public void changeValue()
	{
		pente = (int)slider.value;
        velo.pente(pente);
    }




}
