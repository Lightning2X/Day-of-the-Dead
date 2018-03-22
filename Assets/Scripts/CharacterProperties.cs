﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterProperties : NetworkBehaviour {
	public bool isAlive = true;

    [SerializeField] GameObject head;
    [SerializeField] GameObject torso;
    [SerializeField] GameObject legs;
    Material material;

    [SyncVar]
	public int _health = 1;

    private void Start()
    {
 
    }
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

    public void SetColors(int[] colorIndexes)
    {
        SetObjectColor(head, colorIndexes[0]);
        SetObjectColor(torso, colorIndexes[1]);
        SetObjectColor(legs, colorIndexes[2]);
    }

    private void SetObjectColor(GameObject gameObject, int i)
    {
        material = gameObject.GetComponent<Renderer>().material;
        if (i == 0)
            material.color = new Color(0.690F, 0.125F, 0.082F); //Rood
        else if (i == 1)
            material.color = new Color(0.749F, 0.420F, 0F); //Oranje
        else if (i == 2)
            material.color = new Color(0.071F, 0.534F, 0.722F); //Blauw
        else if (i == 3)
            material.color = new Color(0.392F, 0.690F, 0.278F); //Groen
    }

}