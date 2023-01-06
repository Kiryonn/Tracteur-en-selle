using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public DialogueVelo velo;
	//public GameObject player;
	public int pente;
	public Slider slider;
	public GameObject[] myBatteries;

	private int numberBatteryAvailable = 0;
	private int currentBattery = 0;

	public static GameManager Instance;
	

	void Start()
	{
		if(Instance != null)
        {
			Destroy(this);
        }
        else
        {
			Instance = this;
        }

	}

	// Update is called once per frame
	void Update()
	{

        //player.transform.position += velo.speed *Time.deltaTime * player.transform.forward;

	}

	public void changeValue()
	{
		pente = (int)slider.value;
        velo.pente(pente);
    }

	public void IncreaseBattery(int amount)
    {
		if(currentBattery == myBatteries.Length)
        {
			currentBattery--;

		}
		//Debug.Log(currentBattery);
		var battery = myBatteries[currentBattery];
		
		if (battery.GetComponent<Scrollbar>().size > 0 && !battery.GetComponent<Battery>().IsAvailable)
        {
			numberBatteryAvailable++;
			battery.GetComponent<Battery>().IsAvailable = true;
		}
			
		if (battery.GetComponent<Scrollbar>().size < 1)
			battery.GetComponent<Scrollbar>().size += 0.005f;
		else {
			currentBattery++;
			if (currentBattery < myBatteries.Length) {	
				myBatteries[currentBattery].GetComponent<Scrollbar>().size += 0.005f;
				
			}
			
		}
	}

    public int getNumberBatteryAvailable()
    {
        return numberBatteryAvailable;
    }

    public void UseBattery()
	{
		Debug.Log(numberBatteryAvailable);
		var battery = myBatteries[numberBatteryAvailable-1].GetComponent<Scrollbar>();
		if (battery.size > 0) { 
			battery.size -= 0.005f;
			myBatteries[numberBatteryAvailable - 1].GetComponent<Battery>().IsAvailable = false;
		}
		else {
			if (numberBatteryAvailable - 1 >= 0) {
				myBatteries[numberBatteryAvailable - 1].GetComponent<Scrollbar>().size -= 0.005f;
				myBatteries[numberBatteryAvailable - 1].GetComponent<Battery>().IsAvailable = false;
				numberBatteryAvailable--;
				currentBattery = Math.Max(0, currentBattery - 1);
			}
            
		}
	}
}
