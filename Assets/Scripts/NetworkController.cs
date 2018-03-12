using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkManager
{
    public  void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("player connected");
       // MakingPlayerList();
    }
}
