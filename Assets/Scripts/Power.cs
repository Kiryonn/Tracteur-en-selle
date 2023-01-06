using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{

	public float TimeToDestroyObstacle;

	//Parameters
	private int _LocationNumber = 0;//if there is multiple spot
	private float _timeToDestroyObstacle;
	private Scrollbar JaugePower;

	//define global attributes
	void Start() {
		JaugePower = GetComponent<Scrollbar>();
		ResetObstacle();
	}

	void Update() {
		DialogueVelo dialog = DialogueVelo.Instance;

		JaugePower.size = dialog.speed / dialog.maxSpeed;

		if (JaugePower.size > 0.795f) {
			// high speed
			ChangeColor(Color.red);
			GameManager.Instance.IncreaseBattery(40);
		}
		else if (JaugePower.size > 0.495f) {
			// medium speed
			ChangeColor(Color.yellow);
			GameManager.Instance.IncreaseBattery(20);
		} else {
			// low speed
			ChangeColor(Color.green);
			GameManager.Instance.IncreaseBattery(10);
		}
	}

	public Scrollbar GetJaugeObstacle() {
		return JaugePower;
	}

	private void ChangeColor(Color newcolor) {
		var colors = JaugePower.colors;
		colors.normalColor = newcolor;
		JaugePower.colors = colors;
	}


	//reset to starting value -> should disappear if not using Instantiate and/or using Destroy
	public void ResetObstacle() {
		ColorBlock colors = new ColorBlock();
		colors.normalColor = Color.green;
		
		JaugePower.size = 0f;
		JaugePower.colors = colors;
	}
}