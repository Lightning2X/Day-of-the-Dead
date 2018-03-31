using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client {
	public NetworkConnection connection;
	public GameObject player;
	public bool ready = false;

	public Client (NetworkConnection conn, GameObject plyr) {
		connection = conn;
		player = plyr;
	}
}

public class NetworkController : NetworkManager {
	public List<GameObject> characters = new List<GameObject>();
	public List<GameObject> players = new List<GameObject>();
	GameObject[] characterArray;
	CharacterProperties characterProperties;
	AttributeList attributeList;

	public List<Client> clients = new List<Client>();

	[SerializeField] int minPlayerCount = 2;

	public void OnServerClientReady(NetworkConnection conn) {
		bool allReady = true;
		for (int i = 0; i < clients.Count; i++) {
			if (clients [i].connection.connectionId == conn.connectionId){
				clients [i].ready = true;
			}
			if (!clients [i].ready)
				allReady = false;
		}
		if (allReady)
			OnServerClientsReady ();
	}

	private void OnServerClientsReady() {
		Debug.Log ("spel gestart");
		for (int i = 0; i < clients.Count; i++) {
			clients [i].player.GetComponent<PlayerController> ().resetPosition ();
		}
	}

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

		player.GetComponent<PlayerController>().localConn = conn;
		player.GetComponent<PlayerController>().initialPosition = player.transform.position;

		Client client = new Client (conn, player);

		clients.Add (client);
		players.Add (player);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		if (clients.Count >= minPlayerCount) {
			Debug.Log ("Start pregame");

			characterArray = GameObject.FindGameObjectsWithTag("NPC");
			foreach (GameObject character in characterArray)
			{
				characters.Add(character);
			}
			foreach (GameObject plyr in players)
			{
				characters.Add(plyr);
			}

			shuffleObjectList(characters);
			Debug.Log (characters.Count);
			giveAttributes();
			shuffleObjectList(players);
			giveTarget();

			player.GetComponent<PlayerController> ().LoadPregame ();
		}
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		for (int i = 0; i < clients.Count; i++) {
			if (clients [i].connection.connectionId == conn.connectionId)
				clients.RemoveAt (i);
		}
		for (int i = 0; i < conn.playerControllers.Count; i++) {
			players.Remove (conn.playerControllers [i].gameObject);
		}

		NetworkServer.DestroyPlayersForConnection(conn);
	}

	public override void OnStopServer() {
		clients = new List<Client>();
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect (conn);

	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect (conn);
	}

	private void giveAttributes()
	{
			attributeList = this.GetComponent<AttributeList>();
			for(int i = 0; i < characters.Count; i++)
			{
				characterProperties = characters[i].GetComponent<CharacterProperties>();
			characterProperties.headAttr = attributeList.topAttributes[i];
			characterProperties.torsoAttr = attributeList.middleAttributes[i];
			characterProperties.legsAttr = attributeList.bottomAttributes[i];
				characterProperties.SetColors ();
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
					for(int i = 0; i < players.Count; i++)
					{

							characterProperties = players[i].GetComponent<CharacterProperties>();
							if(i != players.Count - 1)
							{
									characterProperties.target = players[i + 1];
							}
							else
							{
									characterProperties.target = players[0];
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
