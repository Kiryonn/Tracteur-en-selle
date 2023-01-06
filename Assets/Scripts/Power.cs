using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{

	public float TimeToDestroyObstacle;

	//Parameters
	private int _LocationNumber = 0;//if there is multiple spot
	private float _timeToDestroyObstacle;
	private Scrollbar JaugePower;
	private ColorBlock colorPower;

	//define global attributes
	void Start() {
		JaugePower = GetComponent<Scrollbar>();
		colorPower = JaugePower.colors;
		ResetObstacle();
	}

	void Update() {
		JaugePower.size = DialogueVelo.Instance.speed / 350f;
		Debug.Log(JaugePower.size);

		if (JaugePower.size > 0.795f) {
			// high speed
			colorPower.normalColor = Color.red;
			JaugePower.colors = colorPower;
			GameManager.Instance.IncreaseBattery();
		}
		else if (JaugePower.size > 0.495f) {
			// medium speed
			colorPower.normalColor = Color.yellow;
			JaugePower.colors = colorPower;
			GameManager.Instance.IncreaseBattery();
		} else {
			// low speed
			colorPower.normalColor = Color.green;
			JaugePower.colors = colorPower;
		}
		JaugePower.size -= 0.04f * Time.deltaTime;
	}

	public Scrollbar GetJaugeObstacle() {
		return JaugePower;
	}


	//reset to starting value -> should disappear if not using Instantiate and/or using Destroy
	public void ResetObstacle() {
		JaugePower.value = 0;
		colorPower.normalColor = Color.green;
		JaugePower.colors = colorPower;
	}
}