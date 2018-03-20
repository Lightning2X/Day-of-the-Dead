using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreGameEvent : MonoBehaviour {

	public int[] appearance = new int[] {1, 3, 2, 1};

	public Text t1;
	public Text t2;
	public Text t3;
	public Text t4;

	public Button button;

	private bool[] ddReady = new bool[] {false, false, false, false};

	public void OnButtonClick () {
		SceneManager.LoadScene("Main");
	}

	public void OnD1Changed (Dropdown dropdown) {
		if (dropdown.value != 0) {
			t1.text = dropdown.captionText.text;
			if (appearance [0] == dropdown.value)
				t1.color = Color.green;
			else
				t1.color = Color.red;
			dropdown.interactable = false;
			ddReady [0] = true;
			if (ddReady [0] && ddReady [1] && ddReady [2] && ddReady [3])
				button.interactable = true;
		}
	}

	public void OnD2Changed (Dropdown dropdown) {
		if (dropdown.value != 0) {
			t2.text = dropdown.captionText.text;
			if (appearance [1] == dropdown.value)
				t2.color = Color.green;
			else
				t2.color = Color.red;
			dropdown.interactable = false;
			ddReady [1] = true;
			if (ddReady [0] && ddReady [1] && ddReady [2] && ddReady [3])
				button.interactable = true;
		}
	}

	public void OnD3Changed (Dropdown dropdown) {
		if (dropdown.value != 0) {
			t3.text = dropdown.captionText.text;
			if (appearance [2] == dropdown.value)
				t3.color = Color.green;
			else
				t3.color = Color.red;
			dropdown.interactable = false;
			ddReady [2] = true;
			if (ddReady [0] && ddReady [1] && ddReady [2] && ddReady [3])
				button.interactable = true;
		}
	}

	public void OnD4Changed (Dropdown dropdown) {
		if (dropdown.value != 0) {
			t4.text = dropdown.captionText.text;
			if (appearance [3] == dropdown.value)
				t4.color = Color.green;
			else
				t4.color = Color.red;
			dropdown.interactable = false;
			ddReady [3] = true;
			if (ddReady [0] && ddReady [1] && ddReady [2] && ddReady [3])
				button.interactable = true;
		}
	}
}
