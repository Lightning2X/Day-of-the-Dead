﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement_Net : NetworkBehaviour {
    private const float movementSpeed = 0.3f;
    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
            this.transform.position += new Vector3(0, 0, movementSpeed);

        if (Input.GetKey(KeyCode.S))
            this.transform.position -= new Vector3(0, 0, movementSpeed);

        if (Input.GetKey(KeyCode.A))
            this.transform.position -= new Vector3(movementSpeed, 0);
        if (Input.GetKey(KeyCode.D))
            this.transform.position += new Vector3(movementSpeed, 0);
    }
}
