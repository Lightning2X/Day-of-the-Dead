using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttributeList : NetworkBehaviour {

    public List<string> topAttributes = new List<string>();
    public List<string> middleAttributes = new List<string>();
    public List<string> bottomAttributes = new List<string>();
 
    /*public List<GameObject> Players = new List<GameObject>();
    GameObject[] PlayerArray;*/
    // Use this for initialization
    void Start ()
    {
        topAttributes.Add("hat");
        topAttributes.Add("hair");
        middleAttributes.Add("blueshirt");
        middleAttributes.Add("redshirt");
        bottomAttributes.Add("pants");
        bottomAttributes.Add("skirt");
        //MakingPlayerList();
	}

    /*void MakingPlayerList()
    {
        PlayerArray = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(PlayerArray.Length);
        foreach (GameObject player in PlayerArray)
        {
            Players.Add(player);
            Debug.Log("players added to the list" + player);
        }
        Debug.Log("Array Length: " + PlayerArray.Length);
        Debug.Log("List length according to you know" + Players.Count);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        MakingPlayerList();
    }*/
}
