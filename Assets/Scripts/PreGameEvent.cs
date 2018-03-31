using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PreGameEvent : MonoBehaviour {

	public int[] appearance;
	public Text t1;
	public Text t2;
	public Text t3;

	public Button button;

	private Textfields note;
	private PlayerController pc;

	private bool[] ddReady = new bool[] {false, false, false};

	void Start() {
		StartCoroutine(GetAttr());
	}

	IEnumerator GetAttr()
	{
		yield return new WaitForSeconds(0.3f);
		note = GameObject.FindWithTag("Note").GetComponent<Textfields>();
		Cursor.lockState = CursorLockMode.None;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			CharacterProperties cp = players [i].GetComponent<CharacterProperties> ();
			if (cp.isLocalPlayer) {

				pc = players [i].GetComponent<PlayerController> ();
				CharacterProperties tcp = cp.target.GetComponent<CharacterProperties> ();
				appearance = new int[]{tcp.headAttr, tcp.torsoAttr, tcp.legsAttr};
			}
		}
	}

	public void OnButtonClick () {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			PlayerController pc = players [i].GetComponent<PlayerController> ();
			if (pc.hasAuthority) {
				pc.Ready ();
			}
		}
		Cursor.lockState = CursorLockMode.Locked;
		SceneManager.UnloadSceneAsync("PreGame");
	}

	public void OnD1Changed (Dropdown dropdown) {
		if (dropdown.value != 0) {
			t1.text = dropdown.captionText.text;
			note.t1.text = dropdown.captionText.text;
			if (appearance [0] == dropdown.value - 1) {
				t1.color = Color.green;
				note.t1.color = Color.green;
			} else {
				t1.color = Color.red;
				note.t1.color = Color.red;
			}
			pc.CmdSetNoteHead (t1.text, t1.color);
			dropdown.interactable = false;
			ddReady [0] = true;
			if (ddReady [0] && ddReady [1] && ddReady [2])
				button.interactable = true;
		}
	}

	public void OnD2Changed (Dropdown dropdown) {
		if (dropdown.value != 0) {
			t2.text = dropdown.captionText.text;
			note.t2.text = dropdown.captionText.text;
			if (appearance [1] == dropdown.value - 1) {
				t2.color = Color.green;
				note.t2.color = Color.green;
			} else {
				t2.color = Color.red;
				note.t2.color = Color.red;
			}
			pc.CmdSetNoteTorso (t2.text, t2.color);
			dropdown.interactable = false;
			ddReady [1] = true;
			if (ddReady [0] && ddReady [1] && ddReady [2])
				button.interactable = true;
		}
	}

	public void OnD3Changed (Dropdown dropdown) {
		if (dropdown.value != 0) {
			t3.text = dropdown.captionText.text;
			note.t3.text = dropdown.captionText.text;
			if (appearance [2] == dropdown.value - 1) {
				t3.color = Color.green;
				note.t3.color = Color.green;
			} else {
				t3.color = Color.red;
				note.t3.color = Color.red;
			}
			pc.CmdSetNoteLegs (t3.text, t3.color);
			dropdown.interactable = false;
			ddReady [2] = true;
			if (ddReady [0] && ddReady [1] && ddReady [2])
				button.interactable = true;
		}
	}
}
