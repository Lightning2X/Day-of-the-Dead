using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Normal : MonoBehaviour {
    private float movementSpeed = 0.3f;
    private float sprintAmplify = 1.5F;
    private float jumpHeight = 50F;
    private float rotationSpeed = 100F;
    private float cameraDistance = 15f;
    private float cameraHeight = 15f;
    private CharacterController controller;
    Transform mainCamera;
    Vector3 cameraOffset;
    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
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
                controller.Move(transform.forward * sprintAmplify * movementSpeed);
            else
                controller.Move(transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (sprint)
               controller.Move(transform.forward * sprintAmplify * movementSpeed);
            else
                controller.Move(-transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
             this.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
        if (Input.GetKey(KeyCode.D))
            this.transform.Rotate(-Vector3.up * Time.deltaTime * rotationSpeed, Space.World);

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
