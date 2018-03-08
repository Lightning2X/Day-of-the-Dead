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
			GameObject gameObject = Instantiate(spawnPrefab);

			//Spawn the GameObject you assigned in the Inspector
			NetworkServer.Spawn(gameObject);
		}
	}
}
