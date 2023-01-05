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
	private int NumberBatteryFull = 0;

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

	public void IncreaseBattery()
    {
		if(MyBatteries[NumberBatteryFull].GetComponent<Scrollbar>().size > 0)
        {
			NumberBatteryAvailable++;
        }

		if (MyBatteries[NumberBatteryFull].GetComponent<Scrollbar>().size < 1)
		{
			MyBatteries[NumberBatteryFull].GetComponent<Scrollbar>().size += 0.05f;
		}
		else
		{
			if(NumberBatteryFull + 1 <= MyBatteries.Length)
            {
				MyBatteries[NumberBatteryFull + 1].GetComponent<Scrollbar>().size += 0.05f;
				NumberBatteryFull++;
			}
			
		}
	}

	public void UseBattery()
	{
		if (MyBatteries[NumberBatteryFull].GetComponent<Scrollbar>().size < 1)
		{
			MyBatteries[NumberBatteryFull].GetComponent<Scrollbar>().size += 0.05f;
		}
		else
		{
			MyBatteries[NumberBatteryFull + 1].GetComponent<Scrollbar>().size += 0.05f;
			NumberBatteryFull++;
		}
	}




}
