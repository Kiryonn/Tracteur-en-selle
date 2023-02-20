using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueVelo : MonoBehaviour
{

	public FitnessEquipmentDisplay velo;
	public bool connected;
	public float instantaneousPower;
	public float speed;
	public float elapsedTime;
	public int heartRate;
	public int distanceTraveled;
	public int cadence;
	public float maxSpeed = 24f;

	public int veloPente=0;

	// Update is called once per frame
	void Update() {
		lectureVelo();
	}


	void lectureVelo() {
		if (velo.connected) {
			connected = true;
			instantaneousPower = velo.instantaneousPower;
			speed = velo.speed;
			elapsedTime = velo.elapsedTime;
			heartRate = velo.heartRate;
			distanceTraveled = velo.distanceTraveled;
			cadence = velo.cadence;
		}
		if (speed > maxSpeed) {
			speed = maxSpeed;
		}
	}



	public void resitance(int n) {
		velo.SetTrainerResistance(n);
	}

	public void pente(int n) {
		if(n!= veloPente)
			velo.SetTrainerSlope(n);
		veloPente = n;
	}

}
