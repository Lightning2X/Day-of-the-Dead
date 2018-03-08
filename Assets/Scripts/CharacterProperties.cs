using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterProperties : NetworkBehaviour {
	public bool isAlive = true;

	[SyncVar]
	public int _health = 1;


	public int health {
		get {
			return _health;
		}
		set {
			_health = value;
			if (value <= 0)
				isAlive = false;
			else
				isAlive = true;
		}
	}

	[ClientRpc]
	void RpcDamage (int amount)
	{
		Debug.Log ("Your health is now: " + _health);
	}

	public void DealDamage (int amount)
	{
		if (!isServer)
			return;

		health -= amount;

		if (!isAlive)
			NetworkServer.Destroy (gameObject);
	}
}