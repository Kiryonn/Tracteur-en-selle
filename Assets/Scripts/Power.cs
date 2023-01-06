using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{

	public float TimeToDestroyObstacle;

	//Parameters
	private int _LocationNumber = 0;//if there is multiple spot
	private float _timeToDestroyObstacle;
	private Scrollbar JaugePower;
	private ColorBlock colors;
	private	DialogueVelo dialog;
	

	//define global attributes
	void Start() {
		JaugePower = GetComponent<Scrollbar>();
		
		ResetPower();
	}

	void Update() {

		dialog = DialogueVelo.Instance;

			JaugePower.size = dialog.speed / dialog.maxSpeed;
			//JaugePower.size += 0.005f;

			if (JaugePower.size > 0.795f)
			{
				// high speed
				ChangeColor(Color.red);
				GameManager.Instance.IncreaseBattery(40);
			}
			else if (JaugePower.size > 0.495f)
			{
				// medium speed
				ChangeColor(Color.yellow);
				GameManager.Instance.IncreaseBattery(20);
			}
			else
			{
				// low speed
				ChangeColor(Color.green);
				//GameManager.Instance.IncreaseBattery(10);
			}


		


		JaugePower.size -= 0.04f * Time.deltaTime;
	}

	public Scrollbar GetJaugeObstacle() {
		return JaugePower;
	}

	private void ChangeColor(Color newcolor) {
		colors = JaugePower.colors;
		colors.normalColor = newcolor;
		JaugePower.colors = colors;
	}


	//reset to starting value -> should disappear if not using Instantiate and/or using Destroy
	public void ResetPower() {

		JaugePower.size = 0f;
		colors = JaugePower.colors;
		colors.normalColor = Color.green;
		JaugePower.colors = colors;
	}
}