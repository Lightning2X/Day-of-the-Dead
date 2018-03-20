using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkManager {

	public List<GameObject> players = new List<GameObject>();
    //public List<GameObject> targetList = new List<GameObject>();
    public List<GameObject> characters = new List<GameObject>();
    GameObject[] characterArray;
    CharacterProperties characterProperties;
    AttributeList attributeList;


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (playerPrefab == null)
		{
			if (LogFilter.logError) { Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object."); }
			return;
		}

		if (playerPrefab.GetComponent<NetworkIdentity>() == null)
		{
			if (LogFilter.logError) { Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab."); }
			return;
		}

		if (playerControllerId < conn.playerControllers.Count  && conn.playerControllers[playerControllerId].IsValid && conn.playerControllers[playerControllerId].gameObject != null)
		{
			if (LogFilter.logError) { Debug.LogError("There is already a player at that playerControllerId for this connections."); }
			return;
		}

		GameObject player;
		Transform startPos = GetStartPosition();
		if (startPos != null)
		{
			player = (GameObject)Instantiate(playerPrefab, startPos.position, startPos.rotation);
		}
		else
		{
			player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		}
			
		players.Add (player);
        if (characters.Count < 1)
        {
            characterArray = GameObject.FindGameObjectsWithTag("NPC");
            foreach (GameObject character in characterArray)
            {
                characters.Add(character);
            }
            Debug.Log(characterArray.Length);
        }
        characters.Add(player);
        /*targetList.Clear();
        foreach (GameObject p in players)
        {
            targetList.Add(p);
        }*/
        shuffleObjectList(characters);
        giveAttributes();
        shuffleObjectList(players);
        giveTarget();
		//Debug.Log (players.Count);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		for (int i = 0; i < conn.playerControllers.Count; i++) {
			players.Remove (conn.playerControllers[i].gameObject);
            /*if (players.Count > 1)
            {
                characterProperties = conn.playerControllers[i].gameObject.GetComponent<CharacterProperties>();
                foreach (GameObject p in players)
                {
                    CharacterProperties c = p.GetComponent<CharacterProperties>();
                    if (c.Target == conn.playerControllers[i].gameObject)
                        c.Target = characterProperties.Target;
                }
            }*/
		}
		Debug.Log (players.Count);
		NetworkServer.DestroyPlayersForConnection(conn);
	}

	public override void OnStopServer() {
		players = new List<GameObject>();
	}

    private void giveAttributes()
    {
        attributeList = this.GetComponent<AttributeList>();
        for(int i = 0; i < characters.Count; i++)
        {
            characterProperties = characters[i].GetComponent<CharacterProperties>();
            characterProperties.topAttribute = attributeList.topAttributes[i];
            characterProperties.middleAttribute = attributeList.middleAttributes[i];
            characterProperties.bottomAttribute = attributeList.bottomAttributes[i];
        }
        /*int randomNumber = Random.Range(0, attributeList.topAttributes.Count);
        //Debug.Log(attributeList.topAttributes[0]);
        characterProperties.topAttribute = attributeList.topAttributes[randomNumber];
        //Debug.Log(characterProperties.topAttribute);
        attributeList.topAttributes.RemoveAt(randomNumber);
        randomNumber = Random.Range(0, attributeList.middleAttributes.Count);
        characterProperties.middleAttribute = attributeList.middleAttributes[randomNumber];
        attributeList.middleAttributes.RemoveAt(randomNumber);
        randomNumber = Random.Range(0, attributeList.bottomAttributes.Count);
        characterProperties.bottomAttribute = attributeList.bottomAttributes[randomNumber];
        attributeList.bottomAttributes.RemoveAt(randomNumber);*/
    }

    private void giveTarget()
    {
        if (players.Count > 1)
        {
            /*foreach(GameObject player in players)
            {
                Debug.Log(targetList.Count);
                characterProperties = player.GetComponent<CharacterProperties>();
                int randomNumber = Random.Range(0, targetList.Count);
                characterProperties.Target = targetList[randomNumber];
                while (characterProperties.Target == player)
                {
                    randomNumber = Random.Range(0, targetList.Count);
                    characterProperties.Target = targetList[randomNumber];
                    if(players.Count != 2)
                    {
                        CharacterProperties targetProperties = characterProperties.Target.GetComponent<CharacterProperties>();
                        if (targetProperties.Target == player)
                            characterProperties.Target = player;
                    }
                }
                targetList.RemoveAt(randomNumber);
            }*/
            for(int i = 0; i < players.Count; i++)
            {
                characterProperties = players[i].GetComponent<CharacterProperties>();
                if(i != players.Count - 1)
                {
                    characterProperties.Target = players[i + 1];
                }
                else
                {
                    characterProperties.Target = players[0];
                }
            }
        }
    }

    public void shuffleObjectList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
