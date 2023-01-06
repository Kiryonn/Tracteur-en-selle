using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LifeBar : MonoBehaviour
{
    public List<Image> hearts;

	public void Damage() {
		Destroy(hearts[^1]);
		hearts.RemoveAt(hearts.Count - 1);
	}
}
