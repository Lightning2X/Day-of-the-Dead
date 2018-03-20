using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterProperties : NetworkBehaviour {
	public bool isAlive = true;
    public string topAttribute;
    public string middleAttribute;
    public string bottomAttribute;
    public GameObject Target;
    AttributeList attributeList;
    NetworkController networkController;
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

    void Start()
    {
       /* GameObject attributeController = GameObject.Find("AttributeController");
        attributeList = attributeController.GetComponent<AttributeList>();
        GameObject playerListController = GameObject.Find("Network Manager");
        networkController = playerListController.GetComponent<NetworkController>();
        getAttributes();
        getTarget();*/
    }
    private void getAttributes()
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
        int randomNumber = Random.Range(0, attributeList.topAttributes.Count);
        Debug.Log(attributeList.topAttributes[0]);
        topAttribute = attributeList.topAttributes[randomNumber];
        Debug.Log(topAttribute);
        attributeList.topAttributes.RemoveAt(randomNumber);
        randomNumber = Random.Range(0, attributeList.middleAttributes.Count);
        middleAttribute = attributeList.middleAttributes[randomNumber];
        attributeList.middleAttributes.RemoveAt(randomNumber);
        randomNumber = Random.Range(0, attributeList.bottomAttributes.Count);
        bottomAttribute = attributeList.bottomAttributes[randomNumber];
        attributeList.bottomAttributes.RemoveAt(randomNumber);
    }

    private void getTarget()
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
        int randomNumber = Random.Range(0, networkController.players.Count);
        Debug.Log("list length according to properties: " + networkController.players.Count);
        Target = networkController.players[randomNumber];
        if (Target == gameObject)
        {
            getTarget();
        }
        else
        {
            networkController.players.RemoveAt(randomNumber);
        }
    }
}