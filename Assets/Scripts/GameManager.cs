using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public DialogueVelo velo;
	//public GameObject player;
	public int pente;
	public Slider slider;
	public GameObject[] MyBatteries;

	private int NumberBatteryAvailable = 0;
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
		var battery = MyBatteries[currentBattery].GetComponent<Scrollbar>();
		if(battery.size > 0)
			NumberBatteryAvailable++;

		if (battery.size < 1)
		{
			battery.size += 0.05f;
		}
		else {
			if(currentBattery + 1 < MyBatteries.Length)
            {
				MyBatteries[currentBattery + 1].GetComponent<Scrollbar>().size += 0.05f;
				currentBattery++;
			}
			
		}
	}

	public void UseBattery()
	{
		if (MyBatteries[NumberBatteryAvailable].GetComponent<Scrollbar>().size > 0)
		{
			MyBatteries[NumberBatteryAvailable].GetComponent<Scrollbar>().size -= 0.05f;
		}
		else
		{

			if (NumberBatteryAvailable - 1 >= 0)
            {
				MyBatteries[NumberBatteryAvailable - 1].GetComponent<Scrollbar>().size -= 0.05f;
				NumberBatteryAvailable--;
			}
				
		}
	}




}
