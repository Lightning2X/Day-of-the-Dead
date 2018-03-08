using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController_Net : NetworkBehaviour {
    private const float defaultMovementSpeed = 0.3f;
    private const float smokeBombTime = 1f;
    private float movementSpeed;
    private float sprintAmplify = 1.5F;
    private float jumpSpeed = 15F;
    private float rotationSpeed = 100F;
    private float cameraDistance = 15f;
    private float cameraHeight = 15f;
    private CharacterController controller;
    private MeshRenderer meshRenderer;
    Transform mainCamera;
    Vector3 cameraOffset;
    Vector3 moveDirection = Vector3.zero;
    float gravity = -Physics.gravity.y;
    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        controller = GetComponent<CharacterController>();
        meshRenderer = GetComponent<MeshRenderer>();
        movementSpeed = defaultMovementSpeed;
        cameraOffset = new Vector3(0f, cameraHeight, -cameraDistance);
        mainCamera = Camera.main.transform;
        CameraMover();
    }

    // Update is called once per frame
    void Update()
    {
        SmokeBombHandler();
        bool sprint = false;
        if (Input.GetKey(KeyCode.LeftShift))
            sprint = true;

        if (Input.GetKey(KeyCode.W))
        {
            if (sprint)
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
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            moveDirection.y = jumpSpeed;
        if (Input.GetKeyDown(KeyCode.E))
        {
            meshRenderer.enabled = false;
            movementSpeed = 3 * movementSpeed;
            //Add effect etc under here
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        CameraMover();
    }

    private void SmokeBombHandler()
    {
        if(!meshRenderer.enabled)
        {
            StartCoroutine(SmokeBombCountDown());
        }
    }

    IEnumerator SmokeBombCountDown()
    {
        yield return new WaitForSecondsRealtime(smokeBombTime);
        meshRenderer.enabled = true;
        movementSpeed = defaultMovementSpeed;
    }

    void CameraMover()
    {
        mainCamera.position = this.transform.position;
        mainCamera.rotation = this.transform.rotation;
        mainCamera.Translate(cameraOffset);
        mainCamera.LookAt(transform);
    }
}
