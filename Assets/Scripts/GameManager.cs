using UnityEngine;

public class GameManager : MonoBehaviour
{

	public FitnessEquipmentDisplay velo;
	public bool connected;
	public int instantaneousPower;
	public float speed;
	public float elapsedTime;
	public int heartRate;
	public int distanceTraveled;
	public int cadence;

	public GameObject player;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		lectureVelo();
		player.transform.position += speed * Time.deltaTime * player.transform.forward;

	}



	void lectureVelo() 
	{ 
	
		connected = velo.connected;
		instantaneousPower = velo.instantaneousPower;
		speed = velo.speed;
		elapsedTime = velo.elapsedTime;
		heartRate = velo.heartRate;
		distanceTraveled = velo.distanceTraveled;
		cadence = velo.cadence;
	}
}
