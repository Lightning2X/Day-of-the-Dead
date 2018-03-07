using UnityEngine;
using System.Collections;

class Spawner : MonoBehaviour
{

	//Assign the prefab in the Inspector
	public GameObject m_MyGameObject;
	GameObject m_MyInstantiated;


	void OnServerInitialized() {
		Debug.Log ("server initialized");
		//Instantiate the prefab
		//m_MyInstantiated = Instantiate(m_MyGameObject);
		//Spawn the GameObject you assign in the Inspector
		//NetworkServer.Spawn(m_MyInstantiated);
	}
}