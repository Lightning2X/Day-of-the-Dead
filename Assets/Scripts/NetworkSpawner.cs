using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSpawner : NetworkBehaviour {

	[SerializeField] GameObject spawnPrefab;
	[SerializeField] int amount = 1;

	public override void OnStartServer () {
		for (int i = 0; i < amount; i++)
		{
			//Instantiate the prefab
			GameObject gameObject = Instantiate(spawnPrefab, new Vector3(-78, 1, 65), Quaternion.Euler(0, 180, 0));

			//Spawn the GameObject you assigned in the Inspector
			NetworkServer.Spawn(gameObject);
		}
	}
}
