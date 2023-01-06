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
		var battery = myBatteries[currentBattery];
		if(battery.GetComponent<Scrollbar>().size > 0 && !battery.GetComponent<Battery>().IsAvailable)
        {
			numberBatteryAvailable++;
			battery.GetComponent<Battery>().IsAvailable = true;
		}
			
		if (battery.GetComponent<Scrollbar>().size < 1)
			battery.GetComponent<Scrollbar>().size += 0.005f;
		else {
			if(currentBattery + 1 < myBatteries.Length) {
				myBatteries[currentBattery + 1].GetComponent<Scrollbar>().size += 0.005f;
				currentBattery++;
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
		if (battery.size > 0)
			battery.size -= 0.005f;
		else {
			if (numberBatteryAvailable - 1 >= 0) {
				myBatteries[numberBatteryAvailable - 1].GetComponent<Scrollbar>().size -= 0.005f;
				numberBatteryAvailable--;
			}
		}
	}
}
