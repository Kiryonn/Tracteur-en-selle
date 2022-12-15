using UnityEngine;

public class GameManager : MonoBehaviour
{

	public DialogueVelo velo;
	public GameObject player;
	public int pente;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
        velo.pente(pente);

        player.transform.position += velo.speed * Time.deltaTime * player.transform.forward;

	}




}
