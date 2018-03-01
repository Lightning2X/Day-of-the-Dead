using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController_Net : NetworkBehaviour {
    private float movementSpeed = 0.3f;
    private float sprintAmplify = 1.5F;
    private float jumpHeight = 260F;
    private float rotationSpeed = 100F;
    private float cameraDistance = 15f;
    private float cameraHeight = 15f;
    Rigidbody rb;
    Transform mainCamera;
    Vector3 cameraOffset;
    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        rb = GetComponent<Rigidbody>();
        cameraOffset = new Vector3(0f, cameraHeight, -cameraDistance);
        mainCamera = Camera.main.transform;
        CameraMover();
	}
	
	// Update is called once per frame
	void Update () {
        bool sprint = false;
        if (Input.GetKey(KeyCode.LeftShift))
            sprint = true;

        if (Input.GetKey(KeyCode.W))
        {
            if(sprint)
                this.transform.position += transform.forward * sprintAmplify * movementSpeed;
            else
                this.transform.position += transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (sprint)
                this.transform.position += transform.forward * sprintAmplify * movementSpeed;
            else
                this.transform.position += transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.A))
            this.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
        if (Input.GetKey(KeyCode.D))
            this.transform.Rotate(-Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
        if (Input.GetKey(KeyCode.Space))
            transform.Translate(Vector3.up * jumpHeight * Time.deltaTime, Space.World);

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
