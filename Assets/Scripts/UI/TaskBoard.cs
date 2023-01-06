using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class TaskBoard : MonoBehaviour
{
	private Transform tTasks;
	private List<TMP_Text> tasks;
	private int counter = 0;
	// InteractionManager im;
	private void Start() {
		Transform tTasks = transform.Find("Tasks");
	}

	public void AddTask(TMP_Text task) {
		
	}

    public void Complete(int index) {
		TMP_Text task = tasks[index];
		if (task.TryGetComponent<TMP_Text>(out TMP_Text tmp)) {
			tmp.text = "<s>" + tmp.text + "</s>";
		}
	}
}
