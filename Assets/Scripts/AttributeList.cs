using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeList : MonoBehaviour
{

    public List<int> topAttributes = new List<int>();
    public List<int> middleAttributes = new List<int>();
    public List<int> bottomAttributes = new List<int>();
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
            topAttributes.Add(0);
        else if (topListCounter == 1)
            topAttributes.Add(1);
        else if (topListCounter == 2)
            topAttributes.Add(2);
        else if (topListCounter == 3)
            topAttributes.Add(3);

        if (middleListCounter == 0)
            middleAttributes.Add(0);
        else if (middleListCounter == 1)
            middleAttributes.Add(1);
        else if (middleListCounter == 2)
            middleAttributes.Add(2);
        else if (middleListCounter == 3)
            middleAttributes.Add(3);

        if (bottomListCounter == 0)
            bottomAttributes.Add(0);
        else if (bottomListCounter == 1)
            bottomAttributes.Add(1);
        else if (bottomListCounter == 2)
            bottomAttributes.Add(2);
        else if (bottomListCounter == 3)
            bottomAttributes.Add(3);
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

