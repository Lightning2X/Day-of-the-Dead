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
    PlayerList playerList;
	[SyncVar]
	public int _health = 1;


	/*public int health {
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
	}*/

	[ClientRpc]
	void RpcDamage (int amount)
	{
		Debug.Log ("Your health is now: " + _health);
	}

	public void DealDamage (int amount)
	{
		if (!isServer)
			return;

		_health -= amount;
		RpcDamage (amount);
	}
    void Start()
    {
        GameObject attributeController = GameObject.Find("AttributeController");
        attributeList = attributeController.GetComponent<AttributeList>();
        GameObject playerListController = GameObject.Find("PlayerList");
        playerList = playerListController.GetComponent<PlayerList>();
        getAttributes();
        getTarget();
    }
    private void getAttributes()
    {
        int randomNumber = Random.Range(0, attributeList.topAttributes.Count);
        Debug.Log(attributeList.topAttributes[0]);
        topAttribute = attributeList.topAttributes[randomNumber];
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
        int randomNumber = Random.Range(0, playerList.Players.Count);
        Debug.Log("list length according to properties: " + playerList.Players.Count);
        Target = playerList.Players[randomNumber];
        if (Target == gameObject)
        {
            getTarget();
        }
        else
        {
            playerList.Players.RemoveAt(randomNumber);
        }
    }
}