using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkManager {

	public List<GameObject> players = new List<GameObject>();

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

		Debug.Log (players.Count);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		for (int i = 0; i < conn.playerControllers.Count; i++) {
			players.Remove (conn.playerControllers[i].gameObject);
		}
		Debug.Log (players.Count);

		NetworkServer.DestroyPlayersForConnection(conn);
	}

	public override void OnStopServer() {
		players = new List<GameObject>();
	}
}
