using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour {
    public List<GameObject> Players = new List<GameObject>();
    GameObject[] PlayerArray;

    // Use this for initialization
    void Start ()
    {
        MakingPlayerList();
    }
    void MakingPlayerList()
    {
        PlayerArray = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(PlayerArray.Length);
        foreach (GameObject player in PlayerArray)
        {
            Players.Add(player);
            Debug.Log("players added to the list" + player);
        }
        Debug.Log("List length according to you know" + Players.Count);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        MakingPlayerList();
    }
}
