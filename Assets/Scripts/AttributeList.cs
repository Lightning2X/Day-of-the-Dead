using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeList : MonoBehaviour
{

    public List<string> topAttributes = new List<string>();
    public List<string> middleAttributes = new List<string>();
    public List<string> bottomAttributes = new List<string>();
    public int amountTypesAttributes = 4;
    int topListCounter = 0;
    int middleListCounter = 0;
    int bottomListCounter = 0;
    /*public List<GameObject> Players = new List<GameObject>();
    GameObject[] PlayerArray;*/
    // Use this for initialization
    void Start()
    {

        for(bottomListCounter = 0; bottomListCounter < amountTypesAttributes; bottomListCounter++)
        {
            for(middleListCounter = 0; middleListCounter < amountTypesAttributes; middleListCounter++)
            {
                for(topListCounter = 0; topListCounter < amountTypesAttributes; topListCounter++)
                {
                    addToList();
                }
            }
        }
        Debug.Log(topAttributes.Count);
        /*topAttributes.Add("hat");
        topAttributes.Add("hat");
        middleAttributes.Add("blueshirt");
        middleAttributes.Add("redshirt");
        bottomAttributes.Add("pants");
        bottomAttributes.Add("skirt");*/
        //MakingPlayerList();
    }

    void addToList()
    {
        if (topListCounter == 0)
            topAttributes.Add("hat");
        else if (topListCounter == 1)
            topAttributes.Add("longhair");
        else if (topListCounter == 2)
            topAttributes.Add("shorthair");
        else if (topListCounter == 3)
            topAttributes.Add("mask");

        if (middleListCounter == 0)
            middleAttributes.Add("blueshirt");
        else if (middleListCounter == 1)
            middleAttributes.Add("redshirt");
        else if (middleListCounter == 2)
            middleAttributes.Add("dress");
        else if (middleListCounter == 3)
            middleAttributes.Add("bra");

        if (bottomListCounter == 0)
            bottomAttributes.Add("shortpants");
        else if (bottomListCounter == 1)
            bottomAttributes.Add("shortskirt");
        else if (bottomListCounter == 2)
            bottomAttributes.Add("longpants");
        else if (bottomListCounter == 3)
            bottomAttributes.Add("longskirt");
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

