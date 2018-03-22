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
			clients [i].player.transform.position = clients [i].player.GetComponent<PlayerController> ().initialPosition;
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

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		if (clients.Count >= minPlayerCount)
			player.GetComponent<PlayerController> ().LoadPregame ();
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		for (int i = 0; i < clients.Count; i++) {
			if (clients [i].connection.connectionId == conn.connectionId)
				clients.RemoveAt (i);
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
}

