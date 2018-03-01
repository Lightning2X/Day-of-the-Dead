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
            this.transform.position += transform.forward * movementSpeed;
        if (Input.GetKey(KeyCode.S))
            this.transform.position -= transform.forward * movementSpeed;
        if (Input.GetKey(KeyCode.A))
            this.transform.Rotate(Vector3.up * Time.deltaTime * 100f);
        if (Input.GetKey(KeyCode.D))
            this.transform.Rotate(-Vector3.up * Time.deltaTime * 100f);

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
