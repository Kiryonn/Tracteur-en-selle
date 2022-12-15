using UnityEngine;

public class GameManager : MonoBehaviour
{

	public DialogueVelo velo;
	public GameObject player;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		
		player.transform.position += velo.speed * Time.deltaTime * player.transform.forward;

	}




}
