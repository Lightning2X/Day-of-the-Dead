using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController_Net : NetworkBehaviour {
    private float movementSpeed = 0.3f;
    private float cameraDistance = 15f;
    private float cameraHeight = 15f;
    Transform mainCamera;
    Vector3 cameraOffset;
    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        cameraOffset = new Vector3(0f, cameraHeight, -cameraDistance);
        mainCamera = Camera.main.transform;
        CameraMover();
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
        CameraMover();
    }

    void CameraMover()
    {
        mainCamera.position = this.transform.position;
        mainCamera.rotation = this.transform.rotation;
        mainCamera.Translate(cameraOffset);
        mainCamera.LookAt(transform);
    }
}
